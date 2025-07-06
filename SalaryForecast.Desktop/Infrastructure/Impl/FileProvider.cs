using System;
using System.IO;
using Microsoft.Win32;
using SalaryForecast.Core.Db;
using SalaryForecast.Core.Infrastructure;
using SQLite;
using Path = System.IO.Path;

namespace SalaryForecast.Desktop.Infrastructure.Impl
{
    public class FileProvider : IFileProvider
    {
        public string GetJsonDirectory()
        {
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var directoryPath = Path.Combine(baseDirectory, "HolidaysJSON");
            return directoryPath;
        }

        public StreamReader GetJsonFile(int year)
        {
            if (!Directory.Exists(GetJsonDirectory()))
            {
                return null;
            }
            var fileName = $"consultant{year}.json";
            var fullFilePath = Path.Combine(GetJsonDirectory(), fileName);
            return File.Exists(fullFilePath) ? new StreamReader(File.OpenRead(fullFilePath)) : null;
        }

        public string GetDbFilePath()
        {
            var appDataPath =
                (string)Registry.GetValue(
                    "HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Shell Folders",
                    "Common AppData", string.Empty);
            var fullPath = Path.Combine(appDataPath, "Yunker");
            if (!Directory.Exists(fullPath)) Directory.CreateDirectory(fullPath);

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