using System.IO;

namespace SalaryForecast.Core.Infrastructure
{
    public interface IFileProvider
    {
        string GetJsonDirectory();
        StreamReader GetJsonFile(int year);
        string GetDbFilePath();
    }
}