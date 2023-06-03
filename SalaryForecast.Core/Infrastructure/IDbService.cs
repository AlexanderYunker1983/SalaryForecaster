using System.Collections.Generic;
using SalaryForecast.Core.Db;

namespace SalaryForecast.Core.Infrastructure
{
    public interface IDbService
    {
        void Init(string dbPath);
        void Close();
        void AddAdditionalPay(AdditionalPay pay);
        List<AdditionalPay> GetAdditionalPays();
        void DeleteAdditionalPay(AdditionalPay pay);
        void UpdateAdditionalPay(AdditionalPay pay);
    }
}