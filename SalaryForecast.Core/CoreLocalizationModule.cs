using MugenMvvmToolkit;
using MugenMvvmToolkit.Interfaces;
using MugenMvvmToolkit.Interfaces.Models;
using YLocalization;

namespace SalaryForecast.Core
{
    public class CoreLocalizationModule : IModule
    {
        public bool Load(IModuleContext context)
        {
            var iocContainer = context.IocContainer;
            var localizationManager = iocContainer.Get<ILocalizationManager>();
            localizationManager.AddAssembly("SalaryForecast.Core");
            PlatformVariables.LocalizationManager = localizationManager;
            return true;
        }

        public void Unload(IModuleContext context)
        {

        }

        public int Priority => ApplicationSettings.ModulePriorityDefault - 1;
    }
}