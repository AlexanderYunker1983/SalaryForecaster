using SalaryForecast.Core.Models;

namespace SalaryForecast.Core.Infrastructure
{
    public interface ISettingsManager
    {
        int SalaryFirstPartDate { get; set; }
        int SalarySecondPartDate { get; set; }
        decimal Salary { get; set; }
        bool FirstStart { get; set; }
        AppTheme Theme { get; set; }
    }
}
