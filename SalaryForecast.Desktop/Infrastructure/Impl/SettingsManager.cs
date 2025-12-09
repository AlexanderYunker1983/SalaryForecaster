using System.Collections.Generic;
using Newtonsoft.Json;
using SalaryForecast.Core.Infrastructure;
using SalaryForecast.Core.Models;
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

        public IList<RecurringPayment> RecurringPayments
        {
            get
            {
                var raw = Settings.Default.RecurringPaymentsJson;
                if (string.IsNullOrWhiteSpace(raw))
                    return new List<RecurringPayment>();

                try
                {
                    return JsonConvert.DeserializeObject<IList<RecurringPayment>>(raw) ?? new List<RecurringPayment>();
                }
                catch
                {
                    return new List<RecurringPayment>();
                }
            }
            set
            {
                Settings.Default.RecurringPaymentsJson = JsonConvert.SerializeObject(value ?? new List<RecurringPayment>());
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