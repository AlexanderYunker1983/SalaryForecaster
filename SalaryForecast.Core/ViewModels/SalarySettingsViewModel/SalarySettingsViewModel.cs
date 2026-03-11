using SalaryForecast.Core.Infrastructure;

namespace SalaryForecast.Core.ViewModels.SalarySettingsViewModel
{
    public class SalarySettingsViewModel : ViewModelBase
    {
        private readonly ISettingsManager _settingsManager;
        private readonly ILocalizationManager _localizationManager;

        public SalarySettingsViewModel(ISettingsManager settingsManager, ILocalizationManager localizationManager)
        {
            _settingsManager = settingsManager;
            _localizationManager = localizationManager;
        }

        public decimal SalaryValue
        {
            get => _settingsManager.Salary;
            set
            {
                if (_settingsManager.Salary == value) return;
                _settingsManager.Salary = value;
                OnPropertyChanged();
            }
        }

        public int FirstPartDate
        {
            get => _settingsManager.SalaryFirstPartDate;
            set
            {
                if (_settingsManager.SalaryFirstPartDate == value) return;
                _settingsManager.SalaryFirstPartDate = value;
                OnPropertyChanged();
            }
        }

        public int SecondPartDate
        {
            get => _settingsManager.SalarySecondPartDate;
            set
            {
                if (_settingsManager.SalarySecondPartDate == value) return;
                _settingsManager.SalarySecondPartDate = value;
                OnPropertyChanged();
            }
        }

        public string Title => _localizationManager.GetString("SalarySettings");
        public string SalaryValueLabel => _localizationManager.GetString("SalaryValue");
        public string FirstPartDateLabel => _localizationManager.GetString("FirstPartDate");
        public string SecondPartDateLabel => _localizationManager.GetString("SecondPartDate");
        public string SalaryValueToolTip => _localizationManager.GetString("SalaryValueToolTip");
        public string FirstPartDateToolTip => _localizationManager.GetString("FirstPartDateToolTip");
        public string SecondPartDateToolTip => _localizationManager.GetString("SecondPartDateToolTip");
        public string NoteText => _localizationManager.GetString("DataWillBeUpdatedAfterClosing");
    }
}
