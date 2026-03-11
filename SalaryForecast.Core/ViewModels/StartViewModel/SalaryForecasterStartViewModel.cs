using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using SalaryForecast.Core.Infrastructure;
using SalaryForecast.Core.Models;

namespace SalaryForecast.Core.ViewModels.StartViewModel
{
    public class SalaryForecasterStartViewModel : ViewModelBase
    {
        private readonly ILocalizationManager _localizationManager;
        private readonly ISalaryProvider _salaryProvider;
        private readonly ISettingsManager _settingsManager;
        private readonly IMessageService _messageService;
        private readonly ISettingsDialogService _settingsDialogService;
        private readonly IApplicationService _applicationService;
        private string _displayName;
        private bool _showLastYear;
        private string _nextSalaryStatus = string.Empty;
        private ObservableCollection<Salary>? _pastSalaries;
        private ObservableCollection<Salary>? _currentSalaries;
        private bool _isInitialized;

        public SalaryForecasterStartViewModel(
            ILocalizationManager localizationManager,
            ISalaryProvider salaryProvider,
            ISettingsManager settingsManager,
            IMessageService messageService,
            ISettingsDialogService settingsDialogService,
            IApplicationService applicationService,
            IApplicationInfo applicationInfo)
        {
            _localizationManager = localizationManager;
            _salaryProvider = salaryProvider;
            _settingsManager = settingsManager;
            _messageService = messageService;
            _settingsDialogService = settingsDialogService;
            _applicationService = applicationService;
            _displayName = $"{_localizationManager.GetString("ProgramTitle")} v.{applicationInfo.ProgramVersion}";

            OpenSalarySettingsCommand = new AsyncRelayCommand(OpenSalarySettingsAsync);
            ToggleLastYearCommand = new RelayCommand(ToggleLastYear);
        }

        public IAsyncRelayCommand OpenSalarySettingsCommand { get; }
        public IRelayCommand ToggleLastYearCommand { get; }

        public string DisplayName
        {
            get => _displayName;
            private set => SetProperty(ref _displayName, value);
        }

        public bool ShowLastYear
        {
            get => _showLastYear;
            set => SetProperty(ref _showLastYear, value);
        }

        public string NextSalaryStatus
        {
            get => _nextSalaryStatus;
            private set => SetProperty(ref _nextSalaryStatus, value);
        }

        public ObservableCollection<Salary>? PastSalaries
        {
            get => _pastSalaries;
            private set => SetProperty(ref _pastSalaries, value);
        }

        public ObservableCollection<Salary>? CurrentSalaries
        {
            get => _currentSalaries;
            private set => SetProperty(ref _currentSalaries, value);
        }

        public string SettingsMenuTitle => _localizationManager.GetString("Settings");
        public string SalarySettingsMenuTitle => _localizationManager.GetString("SalarySettings");
        public string ViewMenuTitle => _localizationManager.GetString("View");
        public string ToggleLastYearMenuTitle => _localizationManager.GetString("ToggleLastYear");
        public string PreviousYearTitle => _localizationManager.GetString("PreviousYear");
        public string CurrentYearTitle => _localizationManager.GetString("CurrentYear");
        public string DateColumnTitle => _localizationManager.GetString("Date");
        public string SalaryPartValueColumnTitle => _localizationManager.GetString("SalaryPartValue");
        public string SalaryPartPercentColumnTitle => _localizationManager.GetString("SalaryPartPercent");
        public string OneDayCostColumnTitle => _localizationManager.GetString("OneDayCost");
        public string OneHolidayCostColumnTitle => _localizationManager.GetString("OneHolidayCost");
        public string GreenLegendTitle => _localizationManager.GetString("Green");
        public string GreenLegendHelp => _localizationManager.GetString("GreenHelp");

        public async Task InitializeAsync()
        {
            if (_isInitialized) return;
            _isInitialized = true;

            if (_settingsManager.FirstStart)
            {
                _settingsManager.FirstStart = false;
                await _settingsDialogService.ShowSalarySettingsAsync();
            }

            await UpdateCurrentSalariesAsync();
        }

        private async Task UpdateCurrentSalariesAsync()
        {
            var currentYear = DateTime.Now.Year;
            var pastSalaries = await _salaryProvider.GetSalariesAsync(currentYear - 1);
            var currentSalaries = await _salaryProvider.GetSalariesAsync(currentYear);

            if (pastSalaries == null || currentSalaries == null || !pastSalaries.Any() || !currentSalaries.Any())
            {
                PastSalaries = null;
                CurrentSalaries = null;
                await _messageService.ShowErrorAsync(_localizationManager.GetString("Error"),
                    _localizationManager.GetString("CalendarDoesNotExists"));
                _applicationService.Shutdown();
                return;
            }

            var nextSalary = currentSalaries
                .Where(s => s.Date >= DateTime.Now)
                .OrderBy(s => s.Date)
                .FirstOrDefault();

            if (nextSalary == null)
            {
                var nextYearSalaries = await _salaryProvider.GetSalariesAsync(currentYear + 1) ?? new List<Salary>();
                nextSalary = nextYearSalaries
                    .Where(s => s.Date >= DateTime.Now)
                    .OrderBy(s => s.Date)
                    .FirstOrDefault();
            }

            if (nextSalary == null)
            {
                NextSalaryStatus = string.Empty;
                PastSalaries = new ObservableCollection<Salary>(pastSalaries);
                CurrentSalaries = new ObservableCollection<Salary>(currentSalaries);
                return;
            }

            if (currentSalaries.Contains(nextSalary))
            {
                nextSalary.IsNextSalary = true;
            }

            var deltaDays = (nextSalary.Date.Date - DateTime.Now.Date).Days;
            var daysCountString = _localizationManager.GetString("daysMany");
            if (deltaDays / 10 != 1)
            {
                if (deltaDays % 10 == 1) daysCountString = _localizationManager.GetString("daysSingle");
                if (deltaDays % 10 >= 2 && deltaDays % 10 <= 4) daysCountString = _localizationManager.GetString("daysSeveral");
            }

            NextSalaryStatus = $"{_localizationManager.GetString("NextSalaryDays")} {deltaDays} {daysCountString}";
            PastSalaries = new ObservableCollection<Salary>(pastSalaries);
            CurrentSalaries = new ObservableCollection<Salary>(currentSalaries);
        }

        private void ToggleLastYear()
        {
            ShowLastYear = !ShowLastYear;
        }

        private async Task OpenSalarySettingsAsync()
        {
            await _settingsDialogService.ShowSalarySettingsAsync();
            await UpdateCurrentSalariesAsync();
        }
    }
}
