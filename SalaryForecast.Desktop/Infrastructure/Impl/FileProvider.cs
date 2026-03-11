using System;
using System.IO;
using SalaryForecast.Core.Infrastructure;

namespace SalaryForecast.Desktop.Infrastructure.Impl
{
    public class FileProvider : IFileProvider
    {
        public string GetJsonDirectory()
        {
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var directoryPath = Path.Combine(baseDirectory, "HolidaysJSON");
            return directoryPath;
        }

        public StreamReader GetJsonFile(int year)
        {
            if (!Directory.Exists(GetJsonDirectory()))
            {
                return null;
            }
            var fileName = $"consultant{year}.json";
            var fullFilePath = Path.Combine(GetJsonDirectory(), fileName);
            return File.Exists(fullFilePath) ? new StreamReader(File.OpenRead(fullFilePath)) : null;
        }
    }
}
