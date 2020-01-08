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

        public decimal FirstCash
        {
            get => Settings.Default.FirstCash;
            set
            {
                Settings.Default.FirstCash = value;
                Settings.Default.Save();
            }
        }

        public decimal SecondCash
        {
            get => Settings.Default.SecondCash;
            set
            {
                Settings.Default.SecondCash = value;
                Settings.Default.Save();
            }
        }

        public decimal FirstPay
        {
            get => Settings.Default.FirstPay;
            set
            {
                Settings.Default.FirstPay = value;
                Settings.Default.Save();
            }
        }

        public decimal SecondPay
        {
            get => Settings.Default.SecondPay;
            set
            {
                Settings.Default.SecondPay = value;
                Settings.Default.Save();
            }
        }

        public bool FirstStart
        {
            get => Settings.Default.FirstStart;
            set
            {
                Settings.Default.FirstStart = value;
                Settings.Default.Save();
            }
        }
    }
}