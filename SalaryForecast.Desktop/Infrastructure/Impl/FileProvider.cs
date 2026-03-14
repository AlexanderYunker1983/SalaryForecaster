using System;
using System.IO;
using SalaryForecast.Core.Infrastructure;

namespace SalaryForecast.Desktop.Infrastructure.Impl
{
    public class FileProvider : IFileProvider
    {
        private static string GetJsonFilePath(int year)
        {
            return Path.Combine(GetAppDataDirectory(), "HolidaysJSON", $"consultant{year}.json");
        }

        private static string GetAppDataDirectory()
        {
            return Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "Yunker",
                "SalaryForecaster");
        }

        public string GetJsonDirectory()
        {
            return Path.Combine(GetAppDataDirectory(), "HolidaysJSON");
        }

        public bool CalendarFileExists(int year)
        {
            return File.Exists(GetJsonFilePath(year));
        }

        public StreamReader? GetJsonFile(int year)
        {
            var fullFilePath = GetJsonFilePath(year);
            return File.Exists(fullFilePath) ? new StreamReader(File.OpenRead(fullFilePath)) : null;
        }
    }
}
