using System;
using System.Collections.Generic;
using System.Linq;
using SalaryForecast.Core.Models;

namespace SalaryForecast.Core.Infrastructure.Impl
{
    public class CalendarProvider : ICalendarProvider
    {
        private readonly IJsonProvider _jsonProvider;
        private readonly ISettingsManager _settingsManager;

        public CalendarProvider(IFileProvider fileProvider, IJsonProvider jsonProvider, ISettingsManager settingsManager)
        {
            _jsonProvider = jsonProvider;
            _settingsManager = settingsManager;

            _jsonProvider.SetFileProvider(fileProvider);

            Years = new Dictionary<int, Year>();
        }

        public void InitForYear(int year)
        {
            var holidays = _jsonProvider.GetHolidays(year);
            if (holidays != null)
            {
                var holidaysModel = new HolidaysModel(holidays);
                PrepareYear(year, holidaysModel);
            }
        }

        private void PrepareYear(int year, HolidaysModel holidays)
        {
            if (Years.ContainsKey(year)) Years.Remove(year);

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

                var monthModelNearestSalaryFirstPartDateExists = monthModel.Days.Any(d =>
                    d.Key <= _settingsManager.SalaryFirstPartDate && d.Value.IsWorkDate);
                if (monthModelNearestSalaryFirstPartDateExists)
                    monthModel.NearestSalaryFirstPartDate = monthModel.Days.Last(d =>
                        d.Key <= _settingsManager.SalaryFirstPartDate && d.Value.IsWorkDate).Key;
                else
                    monthModel.NearestSalaryFirstPartDate = monthModel.Days.First(d =>
                    d.Key > _settingsManager.SalaryFirstPartDate && d.Value.IsWorkDate).Key;


                var monthModelNearestSecondPartDateExists = monthModel.Days.Any(d =>
                    d.Key <= _settingsManager.SalarySecondPartDate && d.Value.IsWorkDate);
                if (monthModelNearestSecondPartDateExists)
                    monthModel.NearestSalarySecondPartDate = monthModel.Days.Last(d =>
                        d.Key <= _settingsManager.SalarySecondPartDate && d.Value.IsWorkDate).Key;
                else
                    monthModel.NearestSalarySecondPartDate = monthModel.Days.First(d =>
                        d.Key > _settingsManager.SalarySecondPartDate && d.Value.IsWorkDate).Key;

                yearModel.Months.Add(monthIndex, monthModel);
            }

            Years.Add(year, yearModel);
        }

        public Dictionary<int, Year> Years { get; set; }
    }
}