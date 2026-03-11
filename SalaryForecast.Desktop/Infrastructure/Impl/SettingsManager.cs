using System;
using System.IO;
using Newtonsoft.Json;
using SalaryForecast.Core.Infrastructure;

namespace SalaryForecast.Desktop.Infrastructure.Impl
{
    public class SettingsManager : ISettingsManager
    {
        private readonly string _settingsPath;
        private DesktopSettings _settings;

        public SettingsManager()
        {
            var appDirectory = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "Yunker",
                "SalaryForecaster");
            Directory.CreateDirectory(appDirectory);

            _settingsPath = Path.Combine(appDirectory, "settings.json");
            _settings = LoadSettings();
        }

        public int SalaryFirstPartDate
        {
            get => _settings.SalaryFirstPartDate;
            set
            {
                if (_settings.SalaryFirstPartDate == value) return;
                _settings.SalaryFirstPartDate = value;
                SaveSettings();
            }
        }

        public int SalarySecondPartDate
        {
            get => _settings.SalarySecondPartDate;
            set
            {
                if (_settings.SalarySecondPartDate == value) return;
                _settings.SalarySecondPartDate = value;
                SaveSettings();
            }
        }

        public decimal Salary
        {
            get => _settings.SalaryValue;
            set
            {
                if (_settings.SalaryValue == value) return;
                _settings.SalaryValue = value;
                SaveSettings();
            }
        }

        public bool FirstStart
        {
            get => _settings.FirstStart;
            set
            {
                if (_settings.FirstStart == value) return;
                _settings.FirstStart = value;
                SaveSettings();
            }
        }

        private DesktopSettings LoadSettings()
        {
            if (!File.Exists(_settingsPath))
            {
                var settings = new DesktopSettings();
                File.WriteAllText(_settingsPath, JsonConvert.SerializeObject(settings, Formatting.Indented));
                return settings;
            }

            var content = File.ReadAllText(_settingsPath);
            return JsonConvert.DeserializeObject<DesktopSettings>(content) ?? new DesktopSettings();
        }

        private void SaveSettings()
        {
            File.WriteAllText(_settingsPath, JsonConvert.SerializeObject(_settings, Formatting.Indented));
        }

        private sealed class DesktopSettings
        {
            public int SalaryFirstPartDate { get; set; } = 10;
            public int SalarySecondPartDate { get; set; } = 25;
            public decimal SalaryValue { get; set; } = 1000m;
            public bool FirstStart { get; set; } = true;
        }
    }
}
