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

        public SalaryProvider(ICalendarProvider calendarProvider, ISettingsManager settingsManager, IDbService dbService, IFileProvider fileProvider)
        {
            _calendarProvider = calendarProvider;
            _settingsManager = settingsManager;
            _dbService = dbService;
            dbService.Init(fileProvider.GetDbFilePath());
        }

        private void InitializeYear(int year)
        {
            // Предыдущий год нужен для рассчёта зарплаты за январь
            _calendarProvider.InitForYear(year - 1);
            _calendarProvider.InitForYear(year);
        }

        public List<Salary> GetSalaries(int year)
        {
            InitializeYear(year);
            if (!_calendarProvider.Years.ContainsKey(year)) return null;
            var result = new List<Salary>();
            var additionalPays = _dbService.GetAdditionalPays().Where(p => p.Year == year).ToList();
            var salaryYearDelta = 0m;
            foreach (var monthPair in _calendarProvider.Years[year].Months)
            {
                var monthAdditionalPays = additionalPays.Where(p => p.Month == monthPair.Key && p.UseInCalculation).ToList();
                var secondPart = (decimal)monthPair.Value.Days.Count(d => d.Value.IsWorkDate && d.Key <= 15) / monthPair.Value.WorkDaysCount;
                var secondPays = monthAdditionalPays.Where(p => p.Part == 2).ToList();
                var secondPay = secondPays.Sum(p => p.Pay);

                var oneDayCost = _settingsManager.Salary * (1.0m / monthPair.Value.WorkDaysCount - 1.0m / 29.3m);
                var oneDayHolidayCost = _settingsManager.Salary / 29.3m;
                
                var salary = new Salary
                {
                    SalaryPart = secondPart * _settingsManager.Salary,
                    SalaryPercent = secondPart * 100.0m,
                    Date = new DateTime(year, monthPair.Key, monthPair.Value.NearestSalarySecondPartDate),
                    SalaryWithoutCash = secondPart * _settingsManager.Salary - _settingsManager.SecondCash,
                    SalaryWithoutCashAndPay = secondPart * _settingsManager.Salary - _settingsManager.SecondCash - _settingsManager.SecondPay - secondPay,
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
                        salaryYearDelta = salary.SalaryYearDelta;
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
                var firstPay = firstPays.Sum(p => p.Pay);
                var firstSalary = new Salary
                {
                    SalaryPart = firstPart * _settingsManager.Salary,
                    SalaryPercent = firstPart * 100.0m,
                    Date = new DateTime(year, monthPair.Key, monthPair.Value.NearestSalaryFirstPartDate),
                    SalaryWithoutCash = firstPart * _settingsManager.Salary - _settingsManager.FirstCash,
                    SalaryWithoutCashAndPay = firstPart * _settingsManager.Salary - _settingsManager.FirstCash - _settingsManager.FirstPay - firstPay,
                    SalaryDelta = "-----",
                    AdditionalPay = firstPay,
                    OneDayCost = oneDayCost,
                    OneHolidayCost = oneDayHolidayCost
                };

                salaryYearDelta += firstSalary.SalaryWithoutCashAndPay;
                firstSalary.SalaryYearDelta = salaryYearDelta;

                result.Add(firstSalary);
                var totalDeltaSalary = firstSalary.SalaryWithoutCashAndPay + salary.SalaryWithoutCashAndPay;
                salary.SalaryDelta = $"{totalDeltaSalary:F2}";
                if (totalDeltaSalary < 0) salary.WarningEnabled = true;
                salaryYearDelta += salary.SalaryWithoutCashAndPay;
                salary.SalaryYearDelta = salaryYearDelta;
                result.Add(salary);
            }
            return result;
        }
    }
}