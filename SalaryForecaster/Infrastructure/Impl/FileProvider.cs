using System.IO;
using SalaryForecast.Core.Db;
using SalaryForecast.Core.Infrastructure;
using SQLite;

namespace SalaryForecaster.Infrastructure.Impl
{
    public class FileProvider : IFileProvider
    {
        private static readonly string DefaultUserDirectory = Android.OS.Environment.ExternalStorageDirectory.Path;

        public StreamReader GetJsonFile(int year)
        {
            throw new System.NotImplementedException();
        }

        public string GetDbFilePath()
        {
            var fullPath = Path.Combine(DefaultUserDirectory, "SalaryForecaster");
            var dbPath = Path.Combine(fullPath, "SalaryForecster.db");
            if (!File.Exists(dbPath))
            {
                File.Create(dbPath).Close();
                var conn = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite);
                conn.CreateTable<AdditionalPay>();
                conn.Close();
                conn.Dispose();
            }
            return dbPath;
        }
    }
}