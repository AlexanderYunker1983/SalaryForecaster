using System;
using System.Reflection;
using SalaryForecast.Core.Infrastructure;

namespace SalaryForecast.Desktop.Infrastructure.Impl
{
    public class ApplicationInfo : IApplicationInfo
    {
        public string ProgramVersion
        {
            get
            {
                var version = Assembly.GetEntryAssembly()?.GetName().Version ?? new Version(1, 0);
                return version.Build <= 0
                    ? $"{version.Major}.{version.Minor}"
                    : $"{version.Major}.{version.Minor}.{version.Build}";
            }
        }
    }
}
