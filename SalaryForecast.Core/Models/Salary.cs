using System;
using MugenMvvmToolkit.Models;

namespace SalaryForecast.Core.Models
{
    public class Salary : NotifyPropertyChangedBase
    {
        private bool isNextSalary;
        private decimal additionalPay;
        public DateTime Date { get; set; }
        public decimal SalaryPart { get; set; }
        public decimal SalaryPercent { get; set; }
        public decimal SalaryWithoutCash { get; set; }
        public decimal SalaryWithoutCashAndPay { get; set; }
        public string SalaryDelta { get; set; }
        public decimal SalaryYearDelta { get; set; }
        public bool WarningEnabled { get; set; }

        public decimal AdditionalPay
        {
            get => additionalPay;
            set
            {
                if (value == additionalPay)
                {
                    return;
                }

                additionalPay = value;
                OnPropertyChanged();
            }
        }

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