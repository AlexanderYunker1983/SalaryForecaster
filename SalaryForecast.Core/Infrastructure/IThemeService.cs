using SalaryForecast.Core.Models;

namespace SalaryForecast.Core.Infrastructure
{
    public interface IThemeService
    {
        void ApplyTheme(AppTheme theme);
    }
}
