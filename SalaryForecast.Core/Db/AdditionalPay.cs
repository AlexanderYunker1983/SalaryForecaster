using SQLite;

namespace SalaryForecast.Core.Db
{
    public class AdditionalPay
    {
        [PrimaryKey, AutoIncrement]
        public long Id { get; set; }
        [NotNull]
        public int Year { get; set; }
        [NotNull]
        public int Month { get; set; }
        [NotNull]
        public int Part { get; set; }
        [NotNull]
        public decimal Pay { get; set; }
        [NotNull]
        public bool UseInCalculation { get; set; } = true;
        [NotNull]
        public bool UseInCalculationOfVacation { get; set; }
        public string Comment { get; set; }
    }
}