using System;

namespace SalaryForecast.Core.Models
{
    public class Salary : MugenMvvmToolkit.Models.NotifyPropertyChangedBase
    {
        private bool _isNextSalary;
        public DateTime Date { get; set; }
        public decimal SalaryPart { get; set; }
        public decimal SalaryPercent { get; set; }
        public decimal OneDayCost { get; set; }
        public decimal OneHolidayCost { get; set; }

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
