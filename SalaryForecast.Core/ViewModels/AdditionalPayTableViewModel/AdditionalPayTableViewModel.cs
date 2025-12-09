using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Windows.Input;
using MugenMvvmToolkit;
using MugenMvvmToolkit.Interfaces.Models;
using MugenMvvmToolkit.ViewModels;
using SalaryForecast.Core.Db;
using SalaryForecast.Core.Infrastructure;
using YLocalization;
using YMugenExtensions.Commands;

namespace SalaryForecast.Core.ViewModels.AdditionalPayTableViewModel
{
    public class AdditionalPayTableViewModel : CloseableViewModel
    {
        private readonly IDbService _dbService;
        private readonly ILocalizationManager _localizationManager;
        private readonly ISettingsManager _settingsManager;
        private AdditionalPay _selectedAdditionalPay;

        public AdditionalPayTableViewModel(IDbService dbService, ILocalizationManager localizationManager, ISettingsManager settingsManager)
        {
            _dbService = dbService;
            _localizationManager = localizationManager;
            _settingsManager = settingsManager;
            AddNewAdditionalPayCommand = new YRelayCommand(OnAddNewAdditionalPay);
            RemoveSelectedAdditionalPayCommand = new YRelayCommand(OnRemoveSelectedPay, () => SelectedAdditionalPay != null, notifiers:new object[]{this});

            Months = new List<KeyValuePair<int, string>>
            {
                new KeyValuePair<int, string>(1, _localizationManager.GetString("January")),
                new KeyValuePair<int, string>(2, _localizationManager.GetString("February")),
                new KeyValuePair<int, string>(3, _localizationManager.GetString("March")),
                new KeyValuePair<int, string>(4, _localizationManager.GetString("April")),
                new KeyValuePair<int, string>(5, _localizationManager.GetString("May")),
                new KeyValuePair<int, string>(6, _localizationManager.GetString("June")),
                new KeyValuePair<int, string>(7, _localizationManager.GetString("July")),
                new KeyValuePair<int, string>(8, _localizationManager.GetString("August")),
                new KeyValuePair<int, string>(9, _localizationManager.GetString("September")),
                new KeyValuePair<int, string>(10, _localizationManager.GetString("October")),
                new KeyValuePair<int, string>(11, _localizationManager.GetString("November")),
                new KeyValuePair<int, string>(12, _localizationManager.GetString("December"))
            };
        }

        private void OnRemoveSelectedPay()
        {
            _dbService.DeleteAdditionalPay(SelectedAdditionalPay);
            AdditionalPays.Remove(SelectedAdditionalPay);
            SelectedAdditionalPay = null;
        }

        private void OnAddNewAdditionalPay()
        {
            var additionalPay = new AdditionalPay
            {
                Year = DateTime.Now.Year,
                Month = DateTime.Now.Month,
                Part = 1
            };
            _dbService.AddAdditionalPay(additionalPay);
            AdditionalPays.Add(additionalPay);
            SelectedAdditionalPay = additionalPay;
        }

        protected override void OnClosed(IDataContext context)
        {
            foreach (var additionalPay in AdditionalPays) _dbService.UpdateAdditionalPay(additionalPay);
            base.OnClosed(context);
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Parts = new List<KeyValuePair<int, string>>
            {
                new KeyValuePair<int, string>(1, string.Format(_localizationManager.GetString("SalaryPartSalary"), _settingsManager.SalaryFirstPartDate)),
                new KeyValuePair<int, string>(2, string.Format(_localizationManager.GetString("SalaryPartAdvance"), _settingsManager.SalarySecondPartDate))
            };
            var dbPays = _dbService.GetAdditionalPays();

            dbPays.Sort((pay, additionalPay) => (1000000*pay.Month + 1000 * pay.Part).CompareTo(1000000 * additionalPay.Month + 1000 * additionalPay.Part));
            
            AdditionalPays.Clear();
            AdditionalPays.AddRange(dbPays);
        }

        public List<KeyValuePair<int, string>> Parts { get; private set; }
        public AdditionalPay SelectedAdditionalPay
        {
            get => _selectedAdditionalPay;
            set
            {
                if (Equals(value, _selectedAdditionalPay)) return;

                _selectedAdditionalPay = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<AdditionalPay> AdditionalPays { get; set; } = new ObservableCollection<AdditionalPay>();
        public ICommand AddNewAdditionalPayCommand { get; set; }
        public ICommand RemoveSelectedAdditionalPayCommand { get; set; }

        public List<KeyValuePair<int, string>> Months { get; }
    }
}