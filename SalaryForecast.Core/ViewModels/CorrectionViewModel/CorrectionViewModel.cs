using System.Threading.Tasks;
using System.Windows.Input;
using MugenMvvmToolkit.Interfaces.Models;
using MugenMvvmToolkit.ViewModels;
using SalaryForecast.Core.Models;
using YLocalization;
using YMugenExtensions.Commands;

namespace SalaryForecast.Core.ViewModels.CorrectionViewModel
{
    public class CorrectionViewModel : CloseableViewModel, IHasDisplayName
    {
        public string DisplayName { get; set; }

        public void SetSalary(Salary newValue)
        {
            DisplayName = _localizationManager.GetString("Correction", newValue.Date);
            SalaryValue = newValue.SalaryYearDelta;
            RealValue = SalaryValue;
        }

        private readonly ILocalizationManager _localizationManager;

        public CorrectionViewModel(ILocalizationManager localizationManager)
        {
            _localizationManager = localizationManager;

            CancelCommand = new AsyncYRelayCommand(Close);
            ApplyCommand = new AsyncYRelayCommand(Apply);
        }

        private Task Apply()
        {
            Result = CalculatedValue;
            return this.CloseAsync();
        }

        private Task Close()
        {
            Result = null;
            return this.CloseAsync();
        }

        private decimal _salaryValue;

        public decimal SalaryValue
        {
            get => _salaryValue;
            set
            {
                if (value == _salaryValue) return;
                _salaryValue = value;
                OnPropertyChanged();
            }
        }

        private decimal _calculatedValue;

        public decimal CalculatedValue
        {
            get => _calculatedValue;
            set
            {
                if (value == _calculatedValue) return;
                _calculatedValue = value;
                OnPropertyChanged();
            }
        }

        private decimal _realValue;

        public decimal RealValue
        {
            get => _realValue;
            set
            {
                if (value == _realValue) return;
                _realValue = value;
                OnPropertyChanged();
                ReCalculateDelta();
            }
        }

        private void ReCalculateDelta()
        {
            CalculatedValue = RealValue - SalaryValue;
        }

        public decimal? Result { get; set; }

        public ICommand CancelCommand { get; set; }
        public ICommand ApplyCommand { get; set; }
    }
}