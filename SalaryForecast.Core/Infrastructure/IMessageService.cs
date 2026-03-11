using System.Threading.Tasks;

namespace SalaryForecast.Core.Infrastructure
{
    public interface IMessageService
    {
        Task ShowErrorAsync(string title, string message);
    }
}
