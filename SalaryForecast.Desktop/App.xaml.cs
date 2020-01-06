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
        private readonly object[] menuStructure = {
            new MenuWithSubItems("Settings",
                new[]
                {
                    MainMenuItems.SalarySettings,
                }),
        };

        public App()
        {
            PlatformVariables.MenuStructure = menuStructure;

            if (Settings.Default.IsNeedToMigrate)
            {
                Settings.Default.Upgrade();
                Settings.Default.IsNeedToMigrate = false;
                Settings.Default.Save();
            }

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
