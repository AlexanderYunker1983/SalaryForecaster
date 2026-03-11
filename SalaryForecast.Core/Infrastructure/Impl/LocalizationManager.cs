using System;
using System.Globalization;
using SalaryForecast.Core.Resources;

namespace SalaryForecast.Core.Infrastructure.Impl
{
    public class LocalizationManager : ILocalizationManager
    {
        private CultureInfo? _culture = CultureInfo.CurrentUICulture;

        public CultureInfo? Culture => _culture;

        public event EventHandler? CultureChanged;

        public void ChangeCulture(CultureInfo cultureInfo)
        {
            if (Equals(_culture, cultureInfo)) return;

            _culture = cultureInfo;
            CultureChanged?.Invoke(this, EventArgs.Empty);
        }

        public string GetString(string key, params object[] parameters)
        {
            var template = LocalizableResources.ResourceManager.GetString(key, _culture) ?? $"?{key}?";
            if (parameters.Length == 0) return template;

            try
            {
                return string.Format(_culture ?? CultureInfo.CurrentCulture, template, parameters);
            }
            catch (FormatException)
            {
                return template;
            }
        }
    }
}
