using System.Threading;
using System.Threading.Tasks;

namespace SalaryForecast.Core.Infrastructure
{
    public interface IFileDownloader
    {
        Task EnsureConsultantFileAsync(int year, string directoryPath, CancellationToken cancellationToken = default);
    }
}
