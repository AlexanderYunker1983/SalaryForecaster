using System.Collections.Generic;
using SalaryForecast.Core.Db;
using SQLite;

namespace SalaryForecast.Core.Infrastructure.Impl
{
    public class DbService : IDbService
    {
        private SQLiteConnection _conn;
        private string _dbFullPath;
        public void Init(string dbPath)
        {
            _dbFullPath = dbPath;
            _conn = new SQLiteConnection(_dbFullPath, SQLiteOpenFlags.ReadWrite);
            MigrateDb();
            _conn.CreateTable<AdditionalPay>();
        }

        private void MigrateDb()
        {
            var ap = GetAdditionalPays();
            _conn.DropTable<AdditionalPay>();
            _conn.CreateTable<AdditionalPay>();
            _conn.InsertAll(ap);
        }

        public void Close()
        {
            if (_conn != null)
            {
                _conn.Close();
                _conn.Dispose();
                _conn = null;
            }
        }

        public void AddAdditionalPay(AdditionalPay pay)
        {
            _conn.Insert(pay);
        }

        public List<AdditionalPay> GetAdditionalPays()
        {
            var pays = _conn.Table<AdditionalPay>().ToList();
            foreach (var pay in pays)
            {
                if (pay.Pay < 0)
                {
                    pay.Pay = -pay.Pay;
                    pay.IsIncome = true;
                }
            }
            return pays;
        }

        public void DeleteAdditionalPay(AdditionalPay pay)
        {
            _conn.Delete(pay);
        }

        public void UpdateAdditionalPay(AdditionalPay pay)
        {
            _conn.Update(pay);
        }
    }
}