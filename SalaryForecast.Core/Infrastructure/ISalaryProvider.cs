using System.Collections.Generic;
using SalaryForecast.Core.Models;

namespace SalaryForecast.Core.Infrastructure
{
    public interface ISalaryProvider
    {
        void InitializeYear(int year);
        List<Salary> GetSalaries(int year);
    }
}