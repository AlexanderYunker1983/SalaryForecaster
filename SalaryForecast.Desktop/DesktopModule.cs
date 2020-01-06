using System.Reflection;
using MugenMvvmToolkit;
using MugenMvvmToolkit.Interfaces;
using MugenMvvmToolkit.Interfaces.Models;
using MugenMvvmToolkit.Models.IoC;

namespace SalaryForecast.Desktop
{
    public class DesktopModule : IModule
    {
        public bool Load(IModuleContext context)
        {
            var version = Assembly.GetAssembly(GetType()).GetName().Version;

            return true;
        }

        public void Unload(IModuleContext context)
        {
        }

        public int Priority => ApplicationSettings.ModulePriorityDefault;
    }
}