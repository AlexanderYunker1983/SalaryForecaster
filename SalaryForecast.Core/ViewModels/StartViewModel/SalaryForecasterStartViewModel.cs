using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using MugenMvvmToolkit;
using MugenMvvmToolkit.Interfaces.Models;
using MugenMvvmToolkit.Interfaces.Navigation;
using MugenMvvmToolkit.Interfaces.Presenters;
using MugenMvvmToolkit.Interfaces.ViewModels;
using MugenMvvmToolkit.Models;
using MugenMvvmToolkit.ViewModels;
using SalaryForecast.Core.Db;
using SalaryForecast.Core.Infrastructure;
using SalaryForecast.Core.Models;
using YLocalization;
using YMugenExtensions.Menu;

namespace SalaryForecast.Core.ViewModels.StartViewModel
{
    public class SalaryForecasterStartViewModel : CloseableViewModel, IHasDisplayName, INavigableViewModel
    {
        private string _displayName;

        public string DisplayName
        {
            get => _displayName;
            private set
            {
                if (value == _displayName) return;
                _displayName = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<IMenuItemViewModel> Menu { get; } = new ObservableCollection<IMenuItemViewModel>();

        private readonly Dictionary<string, IMenuItemViewModel> _mainMenuItems = new Dictionary<string, IMenuItemViewModel>();
        private readonly ILocalizationManager _localizationManager;
        private readonly ISalaryProvider _salaryProvider;
        private readonly IDbService _dbService;
        private readonly ISettingsManager _settingsManager;
        private readonly IMessagePresenter _messagePresenter;
        private string _nextSalaryStatus;
        private string _yearBalance;
        private string _yearBalanceAlternative;

        public SalaryForecasterStartViewModel(ILocalizationManager localizationManager, ISalaryProvider salaryProvider, IDbService dbService,
            ISettingsManager settingsManager, IMessagePresenter messagePresenter)
        {
            _localizationManager = localizationManager;
            _salaryProvider = salaryProvider;
            _dbService = dbService;
            _settingsManager = settingsManager;
            _messagePresenter = messagePresenter;
            DisplayName = $"{_localizationManager.GetString("ProgramTitle")} v.{PlatformVariables.ProgramVersion}";

            CorrectValueCommand = new AsyncRelayCommand<Salary>(OnCorrectRealValue);
        }

        private async Task OnCorrectRealValue(Salary salary)
        {
            var additionalPays = _dbService.GetAdditionalPays();
            using (var vm = GetViewModel<CorrectionViewModel.CorrectionViewModel>())
            {
                vm.SetSalary(salary);
                await vm.ShowAsync();

                if (vm.Result != null)
                {
                    var additionalPay = new AdditionalPay
                    {
                        Comment = _localizationManager.GetString("Correction", salary.Date),
                        Finished = true,
                        Year = salary.Date.Year,
                        Month = salary.Date.Month,
                        Part= salary.Date.Day <= _settingsManager.SalaryFirstPartDate ? 1: 2,
                        Pay = -(decimal)vm.Result,
                        UseInCalculation = true,
                        UseInCalculationOfVacation = true
                    };
                    _dbService.AddAdditionalPay(additionalPay);
                    await UpdateCurrentSalaries();
                }
            }
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            CreateMenuItems();
            Menu.AddRange(ProcessMenu(PlatformVariables.MenuStructure));

            UpdateCurrentSalaries().ConfigureAwait(false);
        }
        
        protected override void OnClosed(IDataContext context)
        {
            _dbService.Close();

            base.OnClosed(context);
        }

        private async Task UpdateCurrentSalaries()
        {
            var currentYear = DateTime.Now.Year;
            PastSalaries = _salaryProvider.GetSalaries(currentYear - 1);
            CurrentSalaries = _salaryProvider.GetSalaries(currentYear);

            if (PastSalaries == null || CurrentSalaries == null ||
                !PastSalaries.Any() || !CurrentSalaries.Any())
            {
                await _messagePresenter.ShowAsync(_localizationManager.GetString("CalendarDoesNotExists"),
                    _localizationManager.GetString("Error"),
                    MessageButton.Ok, MessageImage.Error);
                await this.CloseAsync();
                return;
            }
            foreach (var currentSalary in CurrentSalaries)
            {
                currentSalary.CorrectValueCommand = CorrectValueCommand;
            }
            var currentMonth = DateTime.Now.Month;
            var currentMonthDate = CurrentSalaries.Where(s => s.Date.Month == currentMonth).ToList();
            var salaryInThisMonth = currentMonthDate.FirstOrDefault(s => s.Date >= DateTime.Now);
            if (salaryInThisMonth == null)
            {
                var nextMonthDate = CurrentSalaries.Where(s => s.Date.Month == currentMonth + 1).ToList();
                var salaryInNextMonth = nextMonthDate.FirstOrDefault(s => s.Date >= DateTime.Now);
                if (salaryInNextMonth == null)
                {
                    return;
                }

                salaryInThisMonth = salaryInNextMonth;
            }
            
            var nextSalary = salaryInThisMonth;
            nextSalary.IsNextSalary = true;
            var salaryDate = nextSalary.Date;

            var deltaDays = (salaryDate.Date - DateTime.Now.Date).Days;

            var daysCountString = _localizationManager.GetString("daysMany");
            if (deltaDays / 10 != 1)
            {
                if (deltaDays % 10 == 1) daysCountString = _localizationManager.GetString("daysSingle");
                if (deltaDays % 10 >= 2 && deltaDays % 10 <= 4) daysCountString = _localizationManager.GetString("daysSeveral");
            }

            NextSalaryStatus = $"{_localizationManager.GetString("NextSalaryDays")} {deltaDays} {daysCountString}";

            var yearSum = CurrentSalaries.Sum(s => s.SalaryWithoutCashAndPay);
            var yearSumAlternative = CurrentSalaries.Sum(s => s.SalaryWithoutCashAndPayAlternative);
            YearBalance = $"{_localizationManager.GetString("YearBalance")} {yearSum:F2}";
            YearBalanceAlternative = $"{_localizationManager.GetString("YearBalance")} {yearSumAlternative:F2}";

            OnPropertyChanged(nameof(PastSalaries));
            OnPropertyChanged(nameof(CurrentSalaries));
        }

        private void CreateMenuItems()
        {
            _mainMenuItems.Add(MainMenuItems.SalarySettings, new MenuItemViewModel(_localizationManager.GetString("SalarySettings"), OnOpenSalarySettings));
            _mainMenuItems.Add(MainMenuItems.AdditionalPaysTable, new MenuItemViewModel(_localizationManager.GetString("EditAdditionalPays"), OnEditAdditionalPays));
            _mainMenuItems.Add(MainMenuItems.View, new MenuItemViewModel(_localizationManager.GetString("ToggleLastYear"), OnToggleLastYear));
        }

        private bool _showLastYear;

        public bool ShowLastYear
        {
            get => _showLastYear;
            set
            {
                if (value == _showLastYear) return;
                _showLastYear = value;
                OnPropertyChanged();
            }
        }

        private Task OnToggleLastYear()
        {
            ShowLastYear = !ShowLastYear;
            return Empty.Task;
        }

        private async Task OnEditAdditionalPays()
        {
            using (var vm = GetViewModel<AdditionalPayTableViewModel.AdditionalPayTableViewModel>())
            {
                await vm.ShowAsync();
            }
            await UpdateCurrentSalaries();
        }

        private async Task OnOpenSalarySettings()
        {
            using (var vm = GetViewModel<SalarySettingsViewModel.SalarySettingsViewModel>())
            {
                await vm.ShowAsync();
            }
            await UpdateCurrentSalaries();
        }

        private IEnumerable<IMenuItemViewModel> ProcessMenu(object[] menuStructure)
        {
            var result = new List<IMenuItemViewModel>();
            foreach (var menu in menuStructure)
                if (menu is MenuWithSubItems withSubItems)
                {
                    var sub = new SubMenuItemViewModel(_localizationManager.GetString(withSubItems.Caption));
                    var items = ProcessMenu(withSubItems.MenuItems.Cast<object>().ToArray());
                    sub.Items.AddRange(items);
                    result.Add(sub);
                }
                else
                {
                    if (menu.ToString().Equals(MainMenuItems.Separator))
                        result.Add(MenuItemViewModel.NewSeparator());
                    else if (_mainMenuItems.TryGetValue(menu.ToString(), out var menuItem)) result.Add(menuItem);
                }

            return result;
        }

        public string NextSalaryStatus
        {
            get => _nextSalaryStatus;
            private set
            {
                if (value == _nextSalaryStatus) return;

                _nextSalaryStatus = value;
                OnPropertyChanged();
            }
        }

        public string YearBalance
        {
            get => _yearBalance;
            private set
            {
                if (value == _yearBalance) return;

                _yearBalance = value;
                OnPropertyChanged();
            }
        }

        public string YearBalanceAlternative
        {
            get => _yearBalanceAlternative;
            private set
            {
                if (value == _yearBalanceAlternative) return;

                _yearBalanceAlternative = value;
                OnPropertyChanged();
            }
        }

        private List<Salary> _pastSalaries;

        public List<Salary> PastSalaries
        {
            get => _pastSalaries;
            set
            {
                if (Equals(value, _pastSalaries)) return;
                _pastSalaries = value;
                OnPropertyChanged();
            }
        }

        private List<Salary> _currentSalaries;
        private bool _showAdditionalColumns;

        public List<Salary> CurrentSalaries
        {
            get => _currentSalaries;
            set
            {
                if (Equals(value, _currentSalaries)) return;
                _currentSalaries = value;
                OnPropertyChanged();
            }
        }

        public async void OnNavigatedTo(INavigationContext context)
        {
            if (_settingsManager.FirstStart)
            {
                _settingsManager.FirstStart = false;

                using (var vm = GetViewModel<SalarySettingsViewModel.SalarySettingsViewModel>())
                {
                    await vm.ShowAsync();
                }

                using (var vm = GetViewModel<AdditionalPayTableViewModel.AdditionalPayTableViewModel>())
                {
                    await vm.ShowAsync();
                }

                await UpdateCurrentSalaries();
            }
        }

        public Task<bool> OnNavigatingFromAsync(INavigationContext context)
        {
            return Task.FromResult(true);
        }

        public void OnNavigatedFrom(INavigationContext context)
        {
        }

        private AsyncRelayCommand<Salary> CorrectValueCommand { get; }
    }
}