using System;
using System.Collections.Generic;
using System.Linq;
using SalaryForecast.Core.Models;

namespace SalaryForecast.Core.Infrastructure.Impl
{
    public class CalendarProvider : ICalendarProvider
    {
        private readonly IFileProvider fileProvider;
        private readonly IJsonProvider jsonProvider;
        private readonly ISettingsManager settingsManager;

        public CalendarProvider(IFileProvider fileProvider, IJsonProvider jsonProvider, ISettingsManager settingsManager)
        {
            this.fileProvider = fileProvider;
            this.jsonProvider = jsonProvider;
            this.settingsManager = settingsManager;

            this.jsonProvider.SetFileProvider(this.fileProvider);

            Years = new Dictionary<int, Year>();
        }

        public void InitForYear(int year)
        {
            var holidays = jsonProvider.GetHolidays(year);
            if (holidays != null)
            {
                var holidaysModel = new HolidaysModel(holidays);
                PrepareYear(year, holidaysModel);
            }
        }

        private void PrepareYear(int year, HolidaysModel holidays)
        {
            if (Years.ContainsKey(year))
            {
                return;
            }

            var yearModel = new Year
            {
                YearDate = year,
                Months = new Dictionary<int, Month>()

            };

            for (var monthIndex = 1; monthIndex <= 12; monthIndex++)
            {
                var monthModel = new Month
                {
                    Date = monthIndex,
                    Days = new Dictionary<int, Day>()
                };
                var daysCount = DateTime.DaysInMonth(year, monthIndex);

                for (var dayIndex = 1; dayIndex < daysCount + 1; dayIndex++)
                {
                    var dayModel = new Day
                    {
                        Date = new DateTime(year, monthIndex, dayIndex),
                    };

                    var isHoliday = holidays.HolidaysDays.Any(h => h.Date.DayOfYear == dayModel.Date.DayOfYear);
                    dayModel.IsWorkDate = !isHoliday;
                    monthModel.Days.Add(dayIndex, dayModel);
                }

                monthModel.WorkDaysCount = monthModel.Days.Count(d => d.Value.IsWorkDate);

                monthModel.NearestSalaryFirstPartDate = monthModel.Days.Last(d =>
                    d.Key <= settingsManager.SalaryFirstPartDate && d.Value.IsWorkDate).Key;

                monthModel.NearestSalarySecondPartDate = monthModel.Days.Last(d =>
                    d.Key <= settingsManager.SalarySecondPartDate && d.Value.IsWorkDate).Key;
                
                yearModel.Months.Add(monthIndex, monthModel);
            }

            Years.Add(year, yearModel);
        }

        public Dictionary<int, Year> Years { get; set; }
    }
}