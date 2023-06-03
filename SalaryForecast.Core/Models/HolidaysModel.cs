using System;
using System.Collections.Generic;

namespace SalaryForecast.Core.Models
{
    public class HolidaysModel
    {
        public HolidaysModel(Holidays holidays)
        {
            HolidaysDays = new List<Day>();
            foreach (var holidaysHoliday in holidays.holidays)
            {
                var holiday = new Day
                {
                    Date = DateTime.Parse(holidaysHoliday),
                    IsWorkDate = false
                };
                HolidaysDays.Add(holiday);
            }
        }

        public List<Day> HolidaysDays { get; set; }
    }
}