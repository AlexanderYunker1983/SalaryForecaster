using System;
using System.Collections.Generic;
using System.Linq;
using MugenMvvmToolkit.Interfaces.Models;
using MugenMvvmToolkit.ViewModels;
using SalaryForecast.Core.Infrastructure;
using SalaryForecast.Core.Models;
using YLocalization;

namespace SalaryForecast.Core.ViewModels.StartViewModel
{
    public class SalaryForecasterStartViewModel : ViewModelBase, IHasDisplayName
    {
        private readonly ILocalizationManager localizationManager;
        private readonly ISalaryProvider salaryProvider;
        private string daysString;
        private Salary selectedSalary;
        public string DisplayName { get; set; }

        public SalaryForecasterStartViewModel(ILocalizationManager localizationManager, ISalaryProvider salaryProvider)
        {
            this.localizationManager = localizationManager;
            this.salaryProvider = salaryProvider;
            DisplayName = $"{this.localizationManager.GetString("ProgramTitle")} v.{PlatformVariables.ProgramVersion}";
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            SalariesPast = salaryProvider.GetSalaries(DateTime.Now.Year - 1);
            Salaries = salaryProvider.GetSalaries(DateTime.Now.Year);
            OnPropertyChanged(nameof(SalariesPast));
            OnPropertyChanged(nameof(Salaries));

            var currentMonth = DateTime.Now.Month;
            var currentMonthDate = Salaries.Where(s => s.Date.Month == currentMonth).ToList();
            SelectedSalary = Salaries.First(s => s.Date > DateTime.Now);
            var salaryDate = SelectedSalary.Date;

            var deltaDays = (salaryDate.Date - DateTime.Now.Date).Days;

            var daysCountString = "дней";
            if (deltaDays / 10 != 1)
            {
                if (deltaDays % 10 == 1)
                {
                    daysCountString = "день";
                }
                if (deltaDays % 10 >= 2 && deltaDays % 10 <= 4)
                {
                    daysCountString = "дня";
                }
            }

            DaysString = $"{deltaDays} {daysCountString}";
        }

        public List<Salary> SalariesPast { get; set; }
        public List<Salary> Salaries { get; set; }

        public string DaysString
        {
            get => daysString;
            set
            {
                if (value == daysString)
                {
                    return;
                }

                daysString = value;
                OnPropertyChanged();
            }
        }

        public Salary SelectedSalary
        {
            get => selectedSalary;
            set
            {
                if (Equals(value, selectedSalary)) return;
                selectedSalary = value;
                OnPropertyChanged();
            }
        }
    }
}