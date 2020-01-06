using System.Reflection;
using MugenMvvmToolkit;
using MugenMvvmToolkit.Interfaces;
using MugenMvvmToolkit.Interfaces.Models;
using MugenMvvmToolkit.Models.IoC;
using SalaryForecast.Core;

namespace SalaryForecast.Desktop
{
    public class DesktopModule : IModule
    {
        public bool Load(IModuleContext context)
        {
            var version = Assembly.GetAssembly(GetType()).GetName().Version;
            if (version.Major > 0)
            {
                PlatformVariables.ProgramVersion = $"{version.Major}.{version.Minor}.{version.Build}";
            }
            else
            {
                PlatformVariables.ProgramVersion = $"{version.Major}.{version.Minor}.{version.Build}-ALPA";
            }
            return true;
        }

        public void Unload(IModuleContext context)
        {
        }

        public int Priority => ApplicationSettings.ModulePriorityDefault;
    }
}