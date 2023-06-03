using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using MugenMvvmToolkit;
using MugenMvvmToolkit.Interfaces.Models;
using MugenMvvmToolkit.ViewModels;
using SalaryForecast.Core.Db;
using SalaryForecast.Core.Infrastructure;
using YMugenExtensions.Commands;

namespace SalaryForecast.Core.ViewModels.AdditionalPayTableViewModel
{
    public class AdditionalPayTableViewModel : CloseableViewModel
    {
        private readonly IDbService _dbService;
        private AdditionalPay _selectedAdditionalPay;

        public AdditionalPayTableViewModel(IDbService dbService)
        {
            _dbService = dbService;
            AddNewAdditionalPayCommand = new YRelayCommand(OnAddNewAdditionalPay);
            RemoveSelectedAdditionalPayCommand = new YRelayCommand(OnRemoveSelectedPay, () => SelectedAdditionalPay != null, notifiers:new object[]{this});
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

            var dbPays = _dbService.GetAdditionalPays();

            dbPays.Sort((pay, additionalPay) => pay.Pay.CompareTo(additionalPay.Pay));
            dbPays.Sort((pay, additionalPay) => pay.Part.CompareTo(additionalPay.Part));
            dbPays.Sort((pay, additionalPay) => pay.Month.CompareTo(additionalPay.Month));

            AdditionalPays.Clear();
            AdditionalPays.AddRange(dbPays);
        }

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
    }
}