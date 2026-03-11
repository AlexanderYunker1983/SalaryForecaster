using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SalaryForecast.Core.Models;

namespace SalaryForecast.Core.Infrastructure
{
    public interface ISalaryProvider
    {
        Task<List<Salary>?> GetSalariesAsync(int year, CancellationToken cancellationToken = default);
    }
}
