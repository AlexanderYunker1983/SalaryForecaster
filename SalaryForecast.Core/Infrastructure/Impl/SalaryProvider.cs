using System;
using System.Collections.Generic;
using System.Linq;
using SalaryForecast.Core.Models;

namespace SalaryForecast.Core.Infrastructure.Impl
{
    public class SalaryProvider : ISalaryProvider
    {
        private readonly ICalendarProvider _calendarProvider;
        private readonly ISettingsManager _settingsManager;
        private readonly IDbService _dbService;
        private readonly IFileDownloader _fileDownloader;
        private readonly IFileProvider _fileProvider;

        public SalaryProvider(ICalendarProvider calendarProvider, ISettingsManager settingsManager, 
            IDbService dbService, IFileProvider fileProvider, IFileDownloader fileDownloader)
        {
            _calendarProvider = calendarProvider;
            _settingsManager = settingsManager;
            _dbService = dbService;
            _fileDownloader = fileDownloader;
            _fileProvider = fileProvider;
            dbService.Init(_fileProvider.GetDbFilePath());
        }

        private void InitializeYear(int year)
        {
            // Предыдущий год нужен для рассчёта зарплаты за январь
            _calendarProvider.InitForYear(year - 1);
            if (!_calendarProvider.Years.ContainsKey(year - 1))
            {
                _fileDownloader.EnsureConsultantFile(year - 1, _fileProvider.GetJsonDirectory());
                _calendarProvider.InitForYear(year - 1);
            }
            _calendarProvider.InitForYear(year);
            if (!_calendarProvider.Years.ContainsKey(year))
            {
                _fileDownloader.EnsureConsultantFile(year, _fileProvider.GetJsonDirectory());
                _calendarProvider.InitForYear(year);
            }
        }

        public List<Salary> GetSalaries(int year)
        {
            InitializeYear(year);
            
            if (!_calendarProvider.Years.ContainsKey(year)) return null;

            var result = new List<Salary>();
            var additionalPays = _dbService.GetAdditionalPays().Where(p => p.Year == year).ToList();
            var salaryYearDelta = 0m;
            var salaryYearDeltaAlternative = 0m;
            foreach (var monthPair in _calendarProvider.Years[year].Months)
            {
                var monthAdditionalPays = additionalPays.Where(p => p.Month == monthPair.Key).ToList();
                var secondPart = (decimal)monthPair.Value.Days.Count(d => d.Value.IsWorkDate && d.Key <= 15) / monthPair.Value.WorkDaysCount;
                var secondPays = monthAdditionalPays.Where(p => p.Part == 2).ToList();
                var secondPay = secondPays.Where(pp => pp.UseInCalculation).Sum(p => p.Pay);
                var secondPayAlternative = secondPays.Sum(p => p.Pay);

                var oneDayCost = _settingsManager.Salary * (1.0m / monthPair.Value.WorkDaysCount - 1.0m / 29.3m);
                var oneDayHolidayCost = _settingsManager.Salary / 29.3m;
                
                var salary = new Salary
                {
                    SalaryPart = secondPart * _settingsManager.Salary,
                    SalaryPercent = secondPart * 100.0m,
                    Date = new DateTime(year, monthPair.Key, monthPair.Value.NearestSalarySecondPartDate),
                    SalaryWithoutCash = secondPart * _settingsManager.Salary - _settingsManager.SecondCash,
                    SalaryWithoutCashAndPay = secondPart * _settingsManager.Salary - _settingsManager.SecondCash - _settingsManager.SecondPay - secondPay,
                    SalaryWithoutCashAndPayAlternative = secondPart * _settingsManager.Salary - _settingsManager.SecondCash - _settingsManager.SecondPay - secondPayAlternative,
                    AdditionalPay = secondPay,
                    OneDayCost = oneDayCost,
                    OneHolidayCost = oneDayHolidayCost
                };
                KeyValuePair<int, Month> previousMonth;
                if (monthPair.Key == 1)
                {
                    if (!_calendarProvider.Years.ContainsKey(year - 1))
                    {
                        salary.SalaryYearDelta = salary.SalaryWithoutCashAndPay;
                        salary.SalaryYearDeltaAlternative = salary.SalaryWithoutCashAndPayAlternative;
                        salaryYearDelta = salary.SalaryYearDelta;
                        salaryYearDeltaAlternative = salary.SalaryYearDeltaAlternative;
                        result.Add(salary);
                        continue;
                    }

                    previousMonth = _calendarProvider.Years[year - 1].Months.First(p => p.Key == 12);
                }
                else
                {
                    previousMonth = _calendarProvider.Years[year].Months.First(p => p.Key == monthPair.Key - 1);
                }

                var firstPart = (decimal)previousMonth.Value.Days.Count(d => d.Value.IsWorkDate && d.Key > 15) / previousMonth.Value.WorkDaysCount;
                var firstPays = monthAdditionalPays.Where(p => p.Part == 1).ToList();
                var firstPay = firstPays.Where(pp => pp.UseInCalculation).Sum(p => p.Pay);
                var firstPayAlternative = firstPays.Sum(p => p.Pay);
                var firstSalary = new Salary
                {
                    SalaryPart = firstPart * _settingsManager.Salary,
                    SalaryPercent = firstPart * 100.0m,
                    Date = new DateTime(year, monthPair.Key, monthPair.Value.NearestSalaryFirstPartDate),
                    SalaryWithoutCash = firstPart * _settingsManager.Salary - _settingsManager.FirstCash,
                    SalaryWithoutCashAndPay = firstPart * _settingsManager.Salary - _settingsManager.FirstCash - _settingsManager.FirstPay - firstPay,
                    SalaryWithoutCashAndPayAlternative = firstPart * _settingsManager.Salary - _settingsManager.FirstCash - _settingsManager.FirstPay - firstPayAlternative,
                    SalaryDelta = "-----",
                    AdditionalPay = firstPay,
                    OneDayCost = oneDayCost,
                    OneHolidayCost = oneDayHolidayCost
                };

                salaryYearDelta += firstSalary.SalaryWithoutCashAndPay;
                salaryYearDeltaAlternative += firstSalary.SalaryWithoutCashAndPayAlternative;
                firstSalary.SalaryYearDelta = salaryYearDelta;
                firstSalary.SalaryYearDeltaAlternative = salaryYearDeltaAlternative;

                result.Add(firstSalary);
                var totalDeltaSalary = firstSalary.SalaryWithoutCashAndPay + salary.SalaryWithoutCashAndPay;
                salary.SalaryDelta = $"{totalDeltaSalary:F2}";
                if (totalDeltaSalary < 0) salary.WarningEnabled = true;
                salaryYearDelta += salary.SalaryWithoutCashAndPay;
                salaryYearDeltaAlternative += salary.SalaryWithoutCashAndPayAlternative;
                salary.SalaryYearDelta = salaryYearDelta;
                salary.SalaryYearDeltaAlternative = salaryYearDeltaAlternative;
                result.Add(salary);
            }

            foreach (var salary in result)
            {
                salary.MaxDiscount = CalculateMaxDiscount(salary, result);
                salary.MaxDiscountAlternative = CalculateMaxAlternativeDiscount(salary, result);
            }
            return result;
        }

        private decimal CalculateMaxDiscount(Salary salary, List<Salary> allSalaries)
        {
            var salaries = allSalaries.Where(s => s.Date >= salary.Date).ToList();
            if (!salaries.Any()) return 0;
            var salaryAfterCalc = salaries.Min(ss => ss.SalaryYearDelta);
            return salaryAfterCalc < 0 ? 0 : salaryAfterCalc;
        }

        private decimal CalculateMaxAlternativeDiscount(Salary salary, List<Salary> allSalaries)
        {
            var salaries = allSalaries.Where(s => s.Date >= salary.Date).ToList();
            if (!salaries.Any()) return 0;
            var salaryAfterCalc = salaries.Min(ss => ss.SalaryYearDeltaAlternative);
            return salaryAfterCalc < 0 ? 0 : salaryAfterCalc;
        }
    }
}