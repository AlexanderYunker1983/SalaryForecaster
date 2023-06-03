using MugenMvvmToolkit;
using MugenMvvmToolkit.Models;
using YLocalization;

namespace SalaryForecast.Core
{
    public class PlatformVariables
    {
        public static ILocalizationManager LocalizationManager { get; set; }
        public static string ProgramVersion { get; set; }
        public static bool IsWpfPlatform => ServiceProvider.Application.PlatformInfo.Platform == PlatformType.WPF;
        public static object[] MenuStructure { get; set; } = new object[0];
    }
}