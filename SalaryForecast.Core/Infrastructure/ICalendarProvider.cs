using System.Collections.Generic;
using SalaryForecast.Core.Models;

namespace SalaryForecast.Core.Infrastructure
{
    public interface ICalendarProvider
    {
        void InitForYear(int year);
        Dictionary<int, Year> Years { get; set; }
    }
}