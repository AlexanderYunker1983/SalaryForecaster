using System.IO;
using SalaryForecast.Core.Infrastructure;

namespace SalaryForecaster.Infrastructure.Impl
{
    public class FileProvider : IFileProvider
    {
        private static readonly string DefaultUserDirectory = Android.OS.Environment.ExternalStorageDirectory.Path;

        public StreamReader GetJsonFile(int year)
        {
            throw new System.NotImplementedException();
        }

        public string GetDbFilePath()
        {
            return Path.Combine(DefaultUserDirectory, "SalaryForecaster");
        }
    }
}