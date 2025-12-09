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

        private static readonly JsonSerializerSettings PaymentSerializerSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.None,
            NullValueHandling = NullValueHandling.Ignore
        };

        public IList<RecurringPayment> RecurringPayments
        {
            get
            {
                var raw = Settings.Default.RecurringPaymentsJson;
                if (string.IsNullOrWhiteSpace(raw))
                    return new List<RecurringPayment>();

                try
                {
                    var payments = JsonConvert.DeserializeObject<IList<RecurringPayment>>(raw, PaymentSerializerSettings) ?? new List<RecurringPayment>();
                    return payments;
                }
                catch
                {
                    return new List<RecurringPayment>();
                }
            }
            set
            {
                Settings.Default.RecurringPaymentsJson = JsonConvert.SerializeObject(value ?? new List<RecurringPayment>(), PaymentSerializerSettings);
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