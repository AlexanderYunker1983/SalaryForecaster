using SalaryForecast.Core.Models;

namespace SalaryForecast.Core.Infrastructure
{
    public interface IJsonProvider
    {
        Holidays GetHolidays(int year);
        void SetFileProvider(IFileProvider fileProvider);
    }
}