using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Microsoft.Extensions.DependencyInjection;
using SalaryForecast.Core.Infrastructure;
using SalaryForecast.Core.ViewModels.SalarySettingsViewModel;
using SalaryForecast.Desktop.Views;

namespace SalaryForecast.Desktop.Infrastructure.Impl
{
    public class SettingsDialogService : ISettingsDialogService
    {
        private readonly IServiceProvider _serviceProvider;

        public SettingsDialogService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task ShowSalarySettingsAsync()
        {
            var window = new SalarySettingsWindow
            {
                DataContext = _serviceProvider.GetRequiredService<SalarySettingsViewModel>()
            };

            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop &&
                desktop.MainWindow != null)
            {
                await window.ShowDialog(desktop.MainWindow);
            }
            else
            {
                window.Show();
            }
        }
    }
}
