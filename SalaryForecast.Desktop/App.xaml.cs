using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using MugenMvvmToolkit;
using MugenMvvmToolkit.Interfaces;
using MugenMvvmToolkit.Models;
using MugenMvvmToolkit.WPF.Infrastructure;
using SalaryForecast.Core;
using SalaryForecast.Core.Infrastructure;
using SalaryForecast.Desktop.Properties;

namespace SalaryForecast.Desktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private readonly object[] _menuStructure = {
            new MenuWithSubItems("Settings",
                new[]
                {
                    MainMenuItems.SalarySettings,
                }),
            new MenuWithSubItems("AdditionalPays",
                new[]
                {
                    MainMenuItems.AdditionalPaysTable,
                }),
            new MenuWithSubItems("View",
                new []
                {
                    MainMenuItems.View,
                    MainMenuItems.ShortView
                })
        };

        public App()
        {
            PlatformVariables.MenuStructure = _menuStructure;

            if (Settings.Default.IsNeedToMigrate)
            {
                Settings.Default.Upgrade();
                Settings.Default.IsNeedToMigrate = false;
                Settings.Default.Save();
            }

            // ReSharper disable once ObjectCreationAsStatement
            new BootstrapperEx(this, new AutofacContainer());
        }

    }
    public class BootstrapperEx : Bootstrapper<SalaryForecasterApp>
    {
        public BootstrapperEx(Application application, IIocContainer iocContainer,
            IEnumerable<Assembly> assemblies = null, PlatformInfo platform = null) : base(application, iocContainer,
            assemblies, platform)
        {
        }
    }
}
