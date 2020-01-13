using Android.App;
using SalaryForecast.Core.Infrastructure;

namespace SalaryForecaster.Infrastructure.Impl
{
    public class SettingsManager : ISettingsManager
    {
        private readonly PreferenceProvider _preferenceProvider;

        private const string SalaryFirstPartDateKey  = nameof(SalaryFirstPartDate);
        private const string SalarySecondPartDateKey = nameof(SalarySecondPartDate);
        private const string SalaryKey               = nameof(Salary);
        private const string FirstCashKey            = nameof(FirstCash);
        private const string SecondCashKey           = nameof(SecondCash);
        private const string FirstPayKey             = nameof(FirstPay);
        private const string SecondPayKey            = nameof(SecondPay);
        private const string FirstStartKey           = nameof(FirstStart);

        public SettingsManager()
        {
            _preferenceProvider = new PreferenceProvider(Application.Context);
        }

        public int SalaryFirstPartDate
        {
            get => _preferenceProvider.GetIntPreference(SalaryFirstPartDateKey, 10);
            set => _preferenceProvider.SavePreference(SalaryFirstPartDateKey, value);
        }

        public int SalarySecondPartDate
        {
            get => _preferenceProvider.GetIntPreference(SalarySecondPartDateKey, 25);
            set => _preferenceProvider.SavePreference(SalarySecondPartDateKey, value);
        }

        public decimal Salary
        {
            get => (decimal) _preferenceProvider.GetFloatPreference(SalaryKey, 1000);
            set => _preferenceProvider.SavePreference(SalaryKey, value);
        }

        public decimal FirstCash
        {
            get => (decimal)_preferenceProvider.GetFloatPreference(FirstCashKey, 250);
            set => _preferenceProvider.SavePreference(FirstCashKey, value);
        }

        public decimal SecondCash
        {
            get => (decimal)_preferenceProvider.GetFloatPreference(SecondCashKey, 250);
            set => _preferenceProvider.SavePreference(SecondCashKey, value);
        }

        public decimal FirstPay
        {
            get => (decimal)_preferenceProvider.GetFloatPreference(FirstPayKey, 250);
            set => _preferenceProvider.SavePreference(FirstPayKey, value);
        }

        public decimal SecondPay
        {
            get => (decimal)_preferenceProvider.GetFloatPreference(SecondPayKey, 250);
            set => _preferenceProvider.SavePreference(SecondPayKey, value);
        }

        public bool FirstStart
        {
            get => _preferenceProvider.GetBoolPreference(FirstStartKey, true);
            set => _preferenceProvider.SavePreference(FirstStartKey, value);
        }
    }
}