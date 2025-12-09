namespace SalaryForecast.Core.Infrastructure
{
    public interface ISettingsManager
    {
        int SalaryFirstPartDate { get; set; }
        int SalarySecondPartDate { get; set; }
        decimal Salary { get; set; }
        decimal FirstCash { get; set; }
        decimal SecondCash { get; set; }
        System.Collections.Generic.IList<Models.RecurringPayment> RecurringPayments { get; set; }
        bool FirstStart { get; set; }
    }
}