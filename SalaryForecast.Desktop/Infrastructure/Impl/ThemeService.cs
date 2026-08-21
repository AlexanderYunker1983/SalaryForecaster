using Avalonia;
using Avalonia.Styling;
using SalaryForecast.Core.Infrastructure;
using SalaryForecast.Core.Models;

namespace SalaryForecast.Desktop.Infrastructure.Impl
{
    public class ThemeService : IThemeService
    {
        public void ApplyTheme(AppTheme theme)
        {
            if (Application.Current == null) return;

            Application.Current.RequestedThemeVariant = theme switch
            {
                AppTheme.Light => ThemeVariant.Light,
                AppTheme.Dark => ThemeVariant.Dark,
                _ => ThemeVariant.Default
            };
        }
    }
}
