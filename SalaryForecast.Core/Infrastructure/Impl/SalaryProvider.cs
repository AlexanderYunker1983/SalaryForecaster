using System;
using System.Collections.Generic;
using System.Linq;
using SalaryForecast.Core.Models;

namespace SalaryForecast.Core.Infrastructure.Impl
{
    public class SalaryProvider : ISalaryProvider
    {
        private readonly ICalendarProvider calendarProvider;
        private readonly ISettingsManager settingsManager;
        private readonly IDbService dbService;
        private readonly IFileProvider fileProvider;

        public SalaryProvider(ICalendarProvider calendarProvider, ISettingsManager settingsManager, IDbService dbService, IFileProvider fileProvider)
        {
            this.calendarProvider = calendarProvider;
            this.settingsManager = settingsManager;
            this.dbService = dbService;
            this.fileProvider = fileProvider;
            dbService.Init(fileProvider.GetDbFilePath());
        }

        private void InitializeYear(int year)
        {
            // Предыдущий год нужен для рассчёта зарплаты за январь
            calendarProvider.InitForYear(year - 1);
            calendarProvider.InitForYear(year);
        }

        public List<Salary> GetSalaries(int year)
        {
            InitializeYear(year);
            if (!calendarProvider.Years.ContainsKey(year))
            {
                return null;
            }
            var result = new List<Salary>();
            foreach (var monthPair in calendarProvider.Years[year].Months)
            {
                var secondPart = (decimal)monthPair.Value.Days.Count(d => d.Value.IsWorkDate && d.Key <= 15) / monthPair.Value.WorkDaysCount;
                var salary = new Salary
                {
                    SalaryPart = secondPart * settingsManager.Salary,
                    SalaryPercent = secondPart * 100.0m,
                    Date = new DateTime(year, monthPair.Key, monthPair.Value.NearestSalarySecondPartDate),
                    SalaryWithoutCash = secondPart * settingsManager.Salary - settingsManager.SecondCash,
                    SalaryWithoutCashAndPay = secondPart * settingsManager.Salary - settingsManager.SecondCash - settingsManager.SecondPay
                };
                KeyValuePair<int, Month> previousMonth;
                if (monthPair.Key == 1)
                {
                    if (!calendarProvider.Years.ContainsKey(year - 1))
                    {
                        result.Add(salary);
                        continue;
                    }

                    previousMonth = calendarProvider.Years[year - 1].Months.First(p => p.Key == 12);
                }
                else
                {
                    previousMonth = calendarProvider.Years[year].Months.First(p => p.Key == monthPair.Key - 1);
                }

                var firstPart = (decimal)previousMonth.Value.Days.Count(d => d.Value.IsWorkDate && d.Key > 15) / previousMonth.Value.WorkDaysCount;
                var firstSalary = new Salary
                {
                    SalaryPart = firstPart * settingsManager.Salary,
                    SalaryPercent = firstPart * 100.0m,
                    Date = new DateTime(year, monthPair.Key, monthPair.Value.NearestSalaryFirstPartDate),
                    SalaryWithoutCash = firstPart * settingsManager.Salary - settingsManager.FirstCash,
                    SalaryWithoutCashAndPay = firstPart * settingsManager.Salary - settingsManager.FirstCash - settingsManager.FirstPay,
                    SalaryDelta = "-----"
                };
                result.Add(firstSalary);
                var totalDeltaSalary = firstSalary.SalaryWithoutCashAndPay + salary.SalaryWithoutCashAndPay;
                salary.SalaryDelta = $"{totalDeltaSalary:F2}";
                if (totalDeltaSalary < 0)
                {
                    salary.WarningEnabled = true;
                }
                result.Add(salary);
            }
            return result;
        }
    }
}