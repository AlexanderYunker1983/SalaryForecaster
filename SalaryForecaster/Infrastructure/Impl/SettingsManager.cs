using Android.App;
using SalaryForecast.Core.Infrastructure;

namespace SalaryForecaster.Infrastructure.Impl
{
    public class SettingsManager : ISettingsManager
    {
        private readonly PreferenceProvider _preferenceProvider;

        public SettingsManager(PreferenceProvider preferenceProvider)
        {
            _preferenceProvider = new PreferenceProvider(Application.Context);
        }

        public int SalaryFirstPartDate { get; set; }
        public int SalarySecondPartDate { get; set; }
        public decimal Salary { get; set; }
        public decimal FirstCash { get; set; }
        public decimal SecondCash { get; set; }
        public decimal FirstPay { get; set; }
        public decimal SecondPay { get; set; }
        public bool FirstStart { get; set; }
    }
}