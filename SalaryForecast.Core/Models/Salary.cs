using System;
using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;

namespace SalaryForecast.Core.Models
{
    public class Salary : ObservableObject
    {
        private static readonly CultureInfo RuCulture = CultureInfo.GetCultureInfo("ru-RU");
        private bool _isNextSalary;

        public DateTime Date { get; set; }
        public decimal SalaryPart { get; set; }
        public decimal SalaryPercent { get; set; }
        public decimal OneDayCost { get; set; }
        public decimal OneHolidayCost { get; set; }

        public bool IsNextSalary
        {
            get => _isNextSalary;
            set => SetProperty(ref _isNextSalary, value);
        }

        public bool IsActive => DateTime.Now.Date < Date;
        public string DateDisplay => Date.ToString("dd MMMM", RuCulture);
        public string SalaryPartDisplay => SalaryPart.ToString("C", RuCulture);
        public string SalaryPercentDisplay => $"{SalaryPercent:F2} %";
        public string OneDayCostDisplay => OneDayCost.ToString("C", RuCulture);
        public string OneHolidayCostDisplay => OneHolidayCost.ToString("C", RuCulture);
    }
}
