using System.Collections.Generic;
using SalaryForecast.Core.Models;

namespace SalaryForecast.Core.Infrastructure
{
    public interface ISalaryProvider
    {
        List<Salary> GetSalaries(int year);
    }
}