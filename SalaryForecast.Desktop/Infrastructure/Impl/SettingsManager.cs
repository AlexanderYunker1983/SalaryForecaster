using SalaryForecast.Core.Infrastructure;
using SalaryForecast.Desktop.Properties;

namespace SalaryForecast.Desktop.Infrastructure.Impl
{
    public class SettingsManager : ISettingsManager
    {
        public int SalaryFirstPartDate
        {
            get => Settings.Default.SalaryFirstPartDate;
            set
            {
                Settings.Default.SalaryFirstPartDate = value;
                Settings.Default.Save();
            }
        }

        public int SalarySecondPartDate
        {
            get => Settings.Default.SalarySecondPartDate;
            set
            {
                Settings.Default.SalarySecondPartDate = value;
                Settings.Default.Save();
            }
        }

        public decimal Salary
        {
            get => Settings.Default.SalaryValue;
            set
            {
                Settings.Default.SalaryValue = value;
                Settings.Default.Save();
            }
        }
    }
}