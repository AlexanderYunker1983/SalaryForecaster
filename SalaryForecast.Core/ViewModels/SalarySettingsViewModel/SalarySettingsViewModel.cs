using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using MugenMvvmToolkit.ViewModels;
using SalaryForecast.Core.Infrastructure;
using SalaryForecast.Core.Models;
using YMugenExtensions.Commands;

namespace SalaryForecast.Core.ViewModels.SalarySettingsViewModel
{
    public class SalarySettingsViewModel : CloseableViewModel
    {
        private readonly ISettingsManager _settingsManager;
        private RecurringPayment _selectedPayment;

        public SalarySettingsViewModel(ISettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
            RecurringPayments = new ObservableCollection<RecurringPayment>(_settingsManager.RecurringPayments ?? Enumerable.Empty<RecurringPayment>());
            AddPaymentCommand = new YRelayCommand(OnAddPayment);
            RemovePaymentCommand = new YRelayCommand(OnRemovePayment, () => SelectedPayment != null, notifiers:new object[]{this});
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

        public ObservableCollection<RecurringPayment> RecurringPayments { get; }

        public RecurringPayment SelectedPayment
        {
            get => _selectedPayment;
            set
            {
                if (Equals(value, _selectedPayment)) return;
                _selectedPayment = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddPaymentCommand { get; }
        public ICommand RemovePaymentCommand { get; }

        protected override void OnClosed(MugenMvvmToolkit.Interfaces.Models.IDataContext context)
        {
            _settingsManager.RecurringPayments = RecurringPayments.ToList();
            base.OnClosed(context);
        }

        private void OnAddPayment()
        {
            var payment = new RecurringPayment
            {
                Name = "Новый платеж",
                Day = 1,
                Amount = 0
            };
            RecurringPayments.Add(payment);
            SelectedPayment = payment;
        }

        private void OnRemovePayment()
        {
            if (SelectedPayment == null) return;
            RecurringPayments.Remove(SelectedPayment);
            SelectedPayment = null;
        }
    }
}