using MugenMvvmToolkit.ViewModels;
using SalaryForecast.Core.Infrastructure;

namespace SalaryForecast.Core.ViewModels.SalarySettingsViewModel
{
    public class SalarySettingsViewModel : CloseableViewModel
    {
        private readonly ISettingsManager _settingsManager;

        public SalarySettingsViewModel(ISettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
        }

        public decimal SalaryValue
        {
            get => _settingsManager.Salary;
            set => _settingsManager.Salary = value;
        }

        public int FirstPartDate
        {
            get => _settingsManager.SalaryFirstPartDate;
            set => _settingsManager.SalaryFirstPartDate = value;
        }

        public int SecondPartDate
        {
            get => _settingsManager.SalarySecondPartDate;
            set => _settingsManager.SalarySecondPartDate = value;
        }

        public decimal FirstCash
        {
            get => _settingsManager.FirstCash;
            set => _settingsManager.FirstCash = value;
        }

        public decimal SecondCash
        {
            get => _settingsManager.SecondCash;
            set => _settingsManager.SecondCash = value;
        }

        public decimal FirstPay
        {
            get => _settingsManager.FirstPay;
            set => _settingsManager.FirstPay = value;
        }

        public decimal SecondPay
        {
            get => _settingsManager.SecondPay;
            set => _settingsManager.SecondPay = value;
        }
    }
}