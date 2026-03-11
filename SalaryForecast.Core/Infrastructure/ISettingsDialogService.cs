using System.Threading.Tasks;

namespace SalaryForecast.Core.Infrastructure
{
    public interface ISettingsDialogService
    {
        Task ShowSalarySettingsAsync();
    }
}
