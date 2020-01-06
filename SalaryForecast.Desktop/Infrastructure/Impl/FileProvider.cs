using System;
using System.IO;
using SalaryForecast.Core.Infrastructure;

namespace SalaryForecast.Desktop.Infrastructure.Impl
{
    public class FileProvider : IFileProvider
    {
        public StreamReader GetJsonFile(int year)
        {
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var directoryPath = Path.Combine(baseDirectory, "HolidaysJSON");
            var fileName = $"consultant{year}.json";
            var fullFilePath = Path.Combine(directoryPath, fileName);
            return File.Exists(fullFilePath) ? new StreamReader(File.Open(fullFilePath, FileMode.Open)) : null;
        }
    }
}