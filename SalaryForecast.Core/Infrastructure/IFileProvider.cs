using System.IO;

namespace SalaryForecast.Core.Infrastructure
{
    public interface IFileProvider
    {
        StreamReader GetJsonFile(int year);
        string GetDbFilePath();
    }
}