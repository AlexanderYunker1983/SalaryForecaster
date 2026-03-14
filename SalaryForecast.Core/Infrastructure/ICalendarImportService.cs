using System.Threading.Tasks;

namespace SalaryForecast.Core.Infrastructure
{
    public interface ICalendarImportService
    {
        Task<bool> TryImportCalendarsAsync();
    }
}
