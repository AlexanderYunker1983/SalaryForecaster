using System.Runtime.Serialization;
using MugenMvvmToolkit.Models;

namespace SalaryForecast.Core.Models
{
    [DataContract]
    public class RecurringPayment : NotifyPropertyChangedBase
    {
        private string _name;
        private int _day;
        private decimal _amount;
        private string _account;

        [DataMember]
        public string Name
        {
            get => _name;
            set
            {
                if (value == _name) return;
                _name = value;
                OnPropertyChanged();
            }
        }

        [DataMember]
        public int Day
        {
            get => _day;
            set
            {
                if (value == _day) return;
                _day = value;
                OnPropertyChanged();
            }
        }

        [DataMember]
        public decimal Amount
        {
            get => _amount;
            set
            {
                if (value == _amount) return;
                _amount = value;
                OnPropertyChanged();
            }
        }

        [DataMember]
        public string Account
        {
            get => _account;
            set
            {
                if (value == _account) return;
                _account = value;
                OnPropertyChanged();
            }
        }
    }
}

