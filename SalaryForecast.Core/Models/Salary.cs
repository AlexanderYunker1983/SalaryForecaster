using System;
using MugenMvvmToolkit.Models;

namespace SalaryForecast.Core.Models
{
    public class Salary : NotifyPropertyChangedBase
    {
        private bool _isNextSalary;
        private decimal _additionalPay;
        public DateTime Date { get; set; }
        public decimal SalaryPart { get; set; }
        public decimal SalaryPercent { get; set; }
        public decimal SalaryWithoutCash { get; set; }
        public decimal SalaryWithoutCashAndPay { get; set; }
        public string SalaryDelta { get; set; }
        public decimal SalaryYearDelta { get; set; }
        public decimal OneDayCost { get; set; }
        public decimal OneHolidayCost { get; set; }
        public bool WarningEnabled { get; set; }

        public decimal AdditionalPay
        {
            get => _additionalPay;
            set
            {
                if (value == _additionalPay) return;

                _additionalPay = value;
                OnPropertyChanged();
            }
        }

        public bool IsNextSalary
        {
            get => _isNextSalary;
            set
            {
                if (value == _isNextSalary) return;

                _isNextSalary = value;
                OnPropertyChanged();
            }
        }

        public bool IsActive
        {
            get
            {
                var currentDate = DateTime.Now.Date;
                return currentDate < Date;
            }
        }
    }
}