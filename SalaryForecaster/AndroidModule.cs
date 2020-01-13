using System.Reflection;
using MugenMvvmToolkit;
using MugenMvvmToolkit.Interfaces;
using MugenMvvmToolkit.Interfaces.Models;
using MugenMvvmToolkit.Models.IoC;
using SalaryForecast.Core;
using SalaryForecast.Core.Infrastructure;

namespace SalaryForecaster
{
    public class AndroidModule : IModule
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
        };

        public bool Load(IModuleContext context)
        {
            var iocContainer = context.IocContainer;
            if (!iocContainer.CanResolve<IFileProvider>()) iocContainer.Bind<IFileProvider, FileProvider>(DependencyLifecycle.SingleInstance);
            if (!iocContainer.CanResolve<ISettingsManager>()) iocContainer.Bind<ISettingsManager, SettingsManager>(DependencyLifecycle.SingleInstance);

            PlatformVariables.MenuStructure = _menuStructure;
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            PlatformVariables.ProgramVersion = version.Build == 0 ? $"{version.Major}.{version.Minor}" : $"{version.Major}.{version.Minor}.{version.Build}-Developer Version";


            return true;
        }

        public void Unload(IModuleContext context)
        {
        }

        public int Priority => ApplicationSettings.ModulePriorityDefault;
    }
}