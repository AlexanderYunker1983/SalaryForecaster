using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using MugenMvvmToolkit;
using MugenMvvmToolkit.Interfaces.Models;
using MugenMvvmToolkit.Interfaces.Navigation;
using MugenMvvmToolkit.Interfaces.ViewModels;
using MugenMvvmToolkit.ViewModels;
using SalaryForecast.Core.Infrastructure;
using SalaryForecast.Core.Models;
using YLocalization;
using YMugenExtensions.Menu;

namespace SalaryForecast.Core.ViewModels.StartViewModel
{
    public class SalaryForecasterStartViewModel : CloseableViewModel, IHasDisplayName, INavigableViewModel
    {
        public string DisplayName { get; set; }
        public ObservableCollection<IMenuItemViewModel> Menu { get; set; } = new ObservableCollection<IMenuItemViewModel>();
        private readonly Dictionary<string, IMenuItemViewModel> mainMenuItems = new Dictionary<string, IMenuItemViewModel>();
        private readonly ILocalizationManager localizationManager;
        private readonly ISalaryProvider salaryProvider;
        private readonly IDbService dbService;
        private readonly ISettingsManager settingsManager;
        private string nextSalaryStatus;
        private string yearBalance;

        public SalaryForecasterStartViewModel(ILocalizationManager localizationManager, ISalaryProvider salaryProvider, IDbService dbService, ISettingsManager settingsManager)
        {
            this.localizationManager = localizationManager;
            this.salaryProvider = salaryProvider;
            this.dbService = dbService;
            this.settingsManager = settingsManager;
            DisplayName = $"{this.localizationManager.GetString("ProgramTitle")} v.{PlatformVariables.ProgramVersion}";
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            CreateMenuItems();
            Menu.AddRange(ProcessMenu(PlatformVariables.MenuStructure));

            UpdateCurrentSalaries();
        }
        
        protected override void OnClosed(IDataContext context)
        {
            dbService.Close();

            base.OnClosed(context);
        }

        private void UpdateCurrentSalaries()
        {
            var currentYear = DateTime.Now.Year;
            PastSalaries = salaryProvider.GetSalaries(currentYear - 1);
            CurrentSalaries = salaryProvider.GetSalaries(currentYear);

            var currentMonth = DateTime.Now.Month;
            var currentMonthDate = CurrentSalaries.Where(s => s.Date.Month == currentMonth).ToList();
            var nextSalary = currentMonthDate.First(s => s.Date >= DateTime.Now);
            nextSalary.IsNextSalary = true;
            var salaryDate = nextSalary.Date;

            var deltaDays = (salaryDate.Date - DateTime.Now.Date).Days;

            var daysCountString = localizationManager.GetString("daysMany");
            if (deltaDays / 10 != 1)
            {
                if (deltaDays % 10 == 1)
                {
                    daysCountString = localizationManager.GetString("daysSingle");
                }
                if (deltaDays % 10 >= 2 && deltaDays % 10 <= 4)
                {
                    daysCountString = localizationManager.GetString("daysSeveral");
                }
            }

            NextSalaryStatus = $"{localizationManager.GetString("NextSalaryDays")} {deltaDays} {daysCountString}";

            var yearSum = CurrentSalaries.Sum(s => s.SalaryWithoutCashAndPay);
            YearBalance = $"{localizationManager.GetString("YearBalance")} {yearSum:F2}";

            OnPropertyChanged(nameof(PastSalaries));
            OnPropertyChanged(nameof(CurrentSalaries));
        }

        private void CreateMenuItems()
        {
            mainMenuItems.Add(MainMenuItems.SalarySettings, new MenuItemViewModel(localizationManager.GetString("SalarySettings"), OnOpenSalarySettings));
            mainMenuItems.Add(MainMenuItems.AdditionalPaysTable, new MenuItemViewModel(localizationManager.GetString("EditAdditionalPays"), OnEditAdditionalPays));
        }

        private async Task OnEditAdditionalPays()
        {
            using (var vm = GetViewModel<AdditionalPayTableViewModel.AdditionalPayTableViewModel>())
            {
                await vm.ShowAsync();
            }
            UpdateCurrentSalaries();
        }

        private async Task OnOpenSalarySettings()
        {
            using (var vm = GetViewModel<SalarySettingsViewModel.SalarySettingsViewModel>())
            {
                await vm.ShowAsync();
            }
            UpdateCurrentSalaries();
        }

        private IEnumerable<IMenuItemViewModel> ProcessMenu(object[] menuStructure)
        {
            var result = new List<IMenuItemViewModel>();
            foreach (var menu in menuStructure)
            {
                if (menu is MenuWithSubItems withSubitems)
                {
                    var sub = new SubMenuItemViewModel(localizationManager.GetString(withSubitems.Caption));
                    var items = ProcessMenu(withSubitems.MenuItems.Cast<object>().ToArray());
                    sub.Items.AddRange(items);
                    result.Add(sub);
                }
                else
                {
                    if (menu.ToString().Equals(MainMenuItems.Separator))
                    {
                        result.Add(MenuItemViewModel.NewSeparator());
                    }
                    else if (mainMenuItems.TryGetValue(menu.ToString(), out var menuItem))
                    {
                        result.Add(menuItem);
                    }
                }
            }
            return result;
        }

        public string NextSalaryStatus
        {
            get => nextSalaryStatus;
            set
            {
                if (value == nextSalaryStatus)
                {
                    return;
                }

                nextSalaryStatus = value;
                OnPropertyChanged();
            }
        }

        public string YearBalance
        {
            get => yearBalance;
            set
            {
                if (value == yearBalance)
                {
                    return;
                }

                yearBalance = value;
                OnPropertyChanged();
            }
        }

        public List<Salary> PastSalaries { get; set; }
        public List<Salary> CurrentSalaries { get; set; }

        public async void OnNavigatedTo(INavigationContext context)
        {
            if (settingsManager.FirstStart)
            {
                settingsManager.FirstStart = false;

                using (var vm = GetViewModel<SalarySettingsViewModel.SalarySettingsViewModel>())
                {
                    await vm.ShowAsync();
                }

                using (var vm = GetViewModel<AdditionalPayTableViewModel.AdditionalPayTableViewModel>())
                {
                    await vm.ShowAsync();
                }

                UpdateCurrentSalaries();
            }
        }

        public Task<bool> OnNavigatingFromAsync(INavigationContext context)
        {
            return Task.FromResult(true);
        }

        public void OnNavigatedFrom(INavigationContext context)
        {
        }
    }
}