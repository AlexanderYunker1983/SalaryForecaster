using System.IO;

namespace SalaryForecast.Core.Infrastructure
{
    public interface IFileProvider
    {
        string GetJsonDirectory();
        bool CalendarFileExists(int year);
        StreamReader? GetJsonFile(int year);
    }
}
