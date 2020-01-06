using MugenMvvmToolkit.ViewModels;
using SalaryForecast.Core.Infrastructure;

namespace SalaryForecast.Core.ViewModels.SalarySettingsViewModel
{
    public class SalarySettingsViewModel : CloseableViewModel
    {
        private readonly ISettingsManager settingsManager;

        public SalarySettingsViewModel(ISettingsManager settingsManager)
        {
            this.settingsManager = settingsManager;
        }

        public decimal SalaryValue
        {
            get => settingsManager.Salary;
            set => settingsManager.Salary = value;
        }

        public int FirstPartDate
        {
            get => settingsManager.SalaryFirstPartDate;
            set => settingsManager.SalaryFirstPartDate = value;
        }

        public int SecondPartDate
        {
            get => settingsManager.SalarySecondPartDate;
            set => settingsManager.SalarySecondPartDate = value;
        }
    }
}