using System.Collections.Generic;

namespace SalaryForecast.Core.Models
{
    public class Year
    {
        public int YearDate { get; set; }
        public Dictionary<int, Month> Months { get; set; }
    }
}