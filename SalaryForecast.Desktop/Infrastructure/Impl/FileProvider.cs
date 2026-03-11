using System;
using System.IO;
using SalaryForecast.Core.Infrastructure;

namespace SalaryForecast.Desktop.Infrastructure.Impl
{
    public class FileProvider : IFileProvider
    {
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

        public StreamReader? GetJsonFile(int year)
        {
            var fullFilePath = Path.Combine(GetJsonDirectory(), $"consultant{year}.json");
            return File.Exists(fullFilePath) ? new StreamReader(File.OpenRead(fullFilePath)) : null;
        }
    }
}
