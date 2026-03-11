using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using SalaryForecast.Core.Infrastructure;
using SalaryForecast.Core.Infrastructure.Impl;
using SalaryForecast.Core.ViewModels.SalarySettingsViewModel;
using SalaryForecast.Core.ViewModels.StartViewModel;
using SalaryForecast.Desktop.Infrastructure.Impl;
using SalaryForecast.Desktop.Views;

namespace SalaryForecast.Desktop
{
    public partial class App : Application
    {
        private ServiceProvider? _services;

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
            _services = ConfigureServices();
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop && _services != null)
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = _services.GetRequiredService<SalaryForecasterStartViewModel>()
                };
            }

            base.OnFrameworkInitializationCompleted();
        }

        private static ServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            services.AddSingleton<ILocalizationManager, LocalizationManager>();
            services.AddSingleton<IJsonProvider, JsonProvider>();
            services.AddSingleton<ICalendarProvider, CalendarProvider>();
            services.AddSingleton<IFileProvider, FileProvider>();
            services.AddSingleton<IFileDownloader, FileDownloader>();
            services.AddSingleton<ISettingsManager, SettingsManager>();
            services.AddSingleton<ISalaryProvider, SalaryProvider>();

            services.AddSingleton<IApplicationInfo, ApplicationInfo>();
            services.AddSingleton<IApplicationService, ApplicationService>();
            services.AddSingleton<IMessageService, MessageService>();
            services.AddSingleton<ISettingsDialogService, SettingsDialogService>();

            services.AddTransient<SalarySettingsViewModel>();
            services.AddSingleton<SalaryForecasterStartViewModel>();

            return services.BuildServiceProvider();
        }
    }
}
