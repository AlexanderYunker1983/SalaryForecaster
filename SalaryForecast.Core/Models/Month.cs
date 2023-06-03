using System.Collections.Generic;

namespace SalaryForecast.Core.Models
{
    public class Month
    {
        public int Date { get; set; }
        public Dictionary<int, Day> Days { get; set; }
        public int WorkDaysCount { get; set; }
        public int NearestSalaryFirstPartDate { get; set; }
        public int NearestSalarySecondPartDate { get; set; }
    }
}