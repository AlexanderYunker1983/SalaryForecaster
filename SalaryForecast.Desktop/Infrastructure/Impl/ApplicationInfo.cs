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
                var assembly = Assembly.GetEntryAssembly();
                var informationalVersion = assembly?
                    .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
                    .InformationalVersion;
                if (!string.IsNullOrWhiteSpace(informationalVersion))
                {
                    return informationalVersion.Split('+')[0];
                }

                var version = assembly?.GetName().Version ?? new Version(1, 0);
                return version.Build <= 0
                    ? $"{version.Major}.{version.Minor}"
                    : $"{version.Major}.{version.Minor}.{version.Build}";
            }
        }
    }
}
