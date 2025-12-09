using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
            RecurringPayments.CollectionChanged += RecurringPaymentsOnCollectionChanged;
            foreach (var payment in RecurringPayments) AttachPayment(payment);
            RecalculateSummary();
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

        public ObservableCollection<RecurringPaymentSummary> PaymentSummaries { get; } = new ObservableCollection<RecurringPaymentSummary>();

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
                Amount = 0,
                Account = string.Empty
            };
            RecurringPayments.Add(payment);
            SelectedPayment = payment;
            SavePayments();
        }

        private void OnRemovePayment()
        {
            if (SelectedPayment == null) return;
            RecurringPayments.Remove(SelectedPayment);
            SelectedPayment = null;
            SavePayments();
        }

        private void RecurringPaymentsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
                foreach (RecurringPayment p in e.NewItems) AttachPayment(p);
            if (e.OldItems != null)
                foreach (RecurringPayment p in e.OldItems) p.PropertyChanged -= PaymentOnPropertyChanged;
            RecalculateSummary();
            SavePayments();
        }

        private void PaymentOnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            RecalculateSummary();
            SavePayments();
        }

        private void AttachPayment(RecurringPayment payment)
        {
            payment.PropertyChanged += PaymentOnPropertyChanged;
        }

        private void RecalculateSummary()
        {
            var firstDate = _settingsManager.SalaryFirstPartDate;
            var secondDate = _settingsManager.SalarySecondPartDate;
            PaymentSummaries.Clear();

            var withAccount = RecurringPayments.Where(p => !string.IsNullOrWhiteSpace(p.Account)).ToList();
            var grouped = withAccount.GroupBy(p => p.Account);
            foreach (var group in grouped)
            {
                var salarySum = group.Where(p => p.Day > firstDate && p.Day <= secondDate).Sum(p => p.Amount);
                var advanceSum = group.Where(p => p.Day <= firstDate || p.Day > secondDate).Sum(p => p.Amount);
                if (salarySum > 0)
                    PaymentSummaries.Add(new RecurringPaymentSummary
                    {
                        Account = group.Key,
                        Period = string.Format(Resources.LocalizableResources.SalaryPartSalary, firstDate),
                        Amount = salarySum
                    });
                if (advanceSum > 0)
                    PaymentSummaries.Add(new RecurringPaymentSummary
                    {
                        Account = group.Key,
                        Period = string.Format(Resources.LocalizableResources.SalaryPartAdvance, _settingsManager.SalarySecondPartDate),
                        Amount = advanceSum
                    });
            }
        }

        private void SavePayments()
        {
            _settingsManager.RecurringPayments = RecurringPayments.ToList();
        }
    }

    public class RecurringPaymentSummary
    {
        public string Account { get; set; }
        public string Period { get; set; }
        public decimal Amount { get; set; }
    }
}