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
        private readonly IFileDownloader _fileDownloader;
        private readonly IFileProvider _fileProvider;

        public SalaryProvider(ICalendarProvider calendarProvider, ISettingsManager settingsManager,
            IFileProvider fileProvider, IFileDownloader fileDownloader)
        {
            _calendarProvider = calendarProvider;
            _settingsManager = settingsManager;
            _fileDownloader = fileDownloader;
            _fileProvider = fileProvider;
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

            foreach (var monthPair in _calendarProvider.Years[year].Months)
            {
                var secondPart = (decimal)monthPair.Value.Days.Count(d => d.Value.IsWorkDate && d.Key <= 15) / monthPair.Value.WorkDaysCount;

                var oneDayCost = _settingsManager.Salary * (1.0m / monthPair.Value.WorkDaysCount - 1.0m / 29.3m);
                var oneDayHolidayCost = _settingsManager.Salary / 29.3m;

                var salary = new Salary
                {
                    SalaryPart = secondPart * _settingsManager.Salary,
                    SalaryPercent = secondPart * 100.0m,
                    Date = new DateTime(year, monthPair.Key, monthPair.Value.NearestSalarySecondPartDate),
                    OneDayCost = oneDayCost,
                    OneHolidayCost = oneDayHolidayCost
                };

                KeyValuePair<int, Month> previousMonth;
                if (monthPair.Key == 1)
                {
                    if (!_calendarProvider.Years.ContainsKey(year - 1))
                    {
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
                var firstSalary = new Salary
                {
                    SalaryPart = firstPart * _settingsManager.Salary,
                    SalaryPercent = firstPart * 100.0m,
                    Date = new DateTime(year, monthPair.Key, monthPair.Value.NearestSalaryFirstPartDate),
                    OneDayCost = oneDayCost,
                    OneHolidayCost = oneDayHolidayCost
                };

                result.Add(firstSalary);
                result.Add(salary);
            }

            return result;
        }
    }
}
