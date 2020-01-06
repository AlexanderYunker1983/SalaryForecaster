using System;

namespace SalaryForecast.Core.Models
{
    public class Salary
    {
        public DateTime Date { get; set; }
        public decimal SalaryPart { get; set; }
        public decimal SalaryPercent { get; set; }
    }
}