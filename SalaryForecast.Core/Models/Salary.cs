using System;
using MugenMvvmToolkit.Models;

namespace SalaryForecast.Core.Models
{
    public class Salary : NotifyPropertyChangedBase
    {
        private bool isNextSalary;
        public DateTime Date { get; set; }
        public decimal SalaryPart { get; set; }
        public decimal SalaryPercent { get; set; }
        public decimal SalaryWithoutCash { get; set; }
        public decimal SalaryWithoutCashAndPay { get; set; }
        public string SalaryDelta { get; set; }
        public bool WarningEnabled { get; set; }
        public bool IsNextSalary
        {
            get => isNextSalary;
            set
            {
                if (value == isNextSalary)
                {
                    return;
                }

                isNextSalary = value;
                OnPropertyChanged();
            }
        }
    }
}