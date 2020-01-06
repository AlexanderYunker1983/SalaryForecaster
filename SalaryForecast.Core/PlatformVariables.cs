using MugenMvvmToolkit;
using MugenMvvmToolkit.Models;

namespace SalaryForecast.Core
{
    public class PlatformVariables
    {
        public static string ProgramVersion { get; set; }
        public static bool IsWpfPlatform => ToolkitServiceProvider.Application.PlatformInfo.Platform == PlatformType.WPF;
        public static object[] MenuStructure { get; set; } = new object[0];
    }
}