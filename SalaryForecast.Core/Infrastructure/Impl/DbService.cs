using System.Collections.Generic;
using SalaryForecast.Core.Db;
using SQLite;

namespace SalaryForecast.Core.Infrastructure.Impl
{
    public class DbService : IDbService
    {
        private SQLiteConnection conn;
        private string dbFullPath;
        public void Init(string dbPath)
        {
            dbFullPath = dbPath;
            conn = new SQLiteConnection(dbFullPath, SQLiteOpenFlags.ReadWrite);

            conn.CreateTable<AdditionalPay>();
        }

        public void Close()
        {
            if (conn != null)
            {
                conn.Close();
                conn.Dispose();
                conn = null;
            }
        }

        public void AddAdditionalPay(AdditionalPay pay)
        {
            conn.Insert(pay);
        }

        public List<AdditionalPay> GetAdditionalPays()
        {
            return conn.Table<AdditionalPay>().ToList();
        }

        public void DeleteAdditionalPay(AdditionalPay pay)
        {
            conn.Delete(pay);
        }

        public void UpdateAdditionalPay(AdditionalPay pay)
        {
            conn.Update(pay);
        }
    }
}