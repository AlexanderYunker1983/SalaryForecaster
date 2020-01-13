using System.IO;
using SalaryForecast.Core.Infrastructure;

namespace SalaryForecaster.Infrastructure.Impl
{
    public class FileProvider : IFileProvider
    {
        public StreamReader GetJsonFile(int year)
        {
            throw new System.NotImplementedException();
        }

        public string GetDbFilePath()
        {
            throw new System.NotImplementedException();
        }
    }
}