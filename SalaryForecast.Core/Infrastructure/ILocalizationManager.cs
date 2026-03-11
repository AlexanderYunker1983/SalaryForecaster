using System;
using System.Globalization;

namespace SalaryForecast.Core.Infrastructure
{
    public interface ILocalizationManager
    {
        CultureInfo? Culture { get; }
        void ChangeCulture(CultureInfo cultureInfo);
        string GetString(string key, params object[] parameters);
        event EventHandler? CultureChanged;
    }
}
