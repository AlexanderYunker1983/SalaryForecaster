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
        private readonly IDbService dbService;
        private AdditionalPay selectedAdditionalPay;

        public AdditionalPayTableViewModel(IDbService dbService)
        {
            this.dbService = dbService;
            AddNewAdditionalPayCommand = new YRelayCommand(OnAddNewAdditionalPay);
            RemoveSelectedAdditionalPayCommand = new YRelayCommand(OnRemoveSelectedPay, () => SelectedAdditionalPay != null, notifiers:new []{this});
        }

        private void OnRemoveSelectedPay()
        {
            dbService.DeleteAdditionalPay(SelectedAdditionalPay);
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
            dbService.AddAdditionalPay(additionalPay);
            AdditionalPays.Add(additionalPay);
            SelectedAdditionalPay = additionalPay;
        }

        protected override void OnClosed(IDataContext context)
        {
            foreach (var additionalPay in AdditionalPays)
            {
                dbService.UpdateAdditionalPay(additionalPay);
            }
            base.OnClosed(context);
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            var dbPays = dbService.GetAdditionalPays();
            AdditionalPays.Clear();
            AdditionalPays.AddRange(dbPays);
        }

        public AdditionalPay SelectedAdditionalPay
        {
            get => selectedAdditionalPay;
            set
            {
                if (Equals(value, selectedAdditionalPay))
                {
                    return;
                }

                selectedAdditionalPay = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<AdditionalPay> AdditionalPays { get; set; } = new ObservableCollection<AdditionalPay>();
        public ICommand AddNewAdditionalPayCommand { get; set; }
        public ICommand RemoveSelectedAdditionalPayCommand { get; set; }
    }
}