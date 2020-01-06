using System.Reflection;
using MugenMvvmToolkit;
using MugenMvvmToolkit.Interfaces;
using MugenMvvmToolkit.Interfaces.Models;
using SalaryForecast.Core;

namespace SalaryForecast.Desktop
{
    public class DesktopModule : IModule
    {
        public bool Load(IModuleContext context)
        {
            var version = Assembly.GetAssembly(GetType()).GetName().Version;
            PlatformVariables.ProgramVersion = version.Build == 0 ? $"{version.Major}.{version.Minor}" : $"{version.Major}.{version.Minor}.{version.Build}-Developer Version";
            return true;
        }

        public void Unload(IModuleContext context)
        {
        }

        public int Priority => ApplicationSettings.ModulePriorityDefault;
    }
}