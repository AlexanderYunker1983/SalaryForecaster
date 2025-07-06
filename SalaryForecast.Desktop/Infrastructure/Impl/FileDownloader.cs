using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using SalaryForecast.Core.Infrastructure;

namespace SalaryForecast.Desktop.Infrastructure.Impl
{
    public class FileDownloader : IFileDownloader
    {
        public void EnsureConsultantFile(int year, string directoryPath)
        {
            string fileName = $"consultant{year}.json";
            string localFilePath = Path.Combine(directoryPath, fileName);
            string githubRawUrl = $"https://raw.githubusercontent.com/d10xa/holidays-calendar/master/json/{fileName}";
            string githubUrl = $"https://github.com/d10xa/holidays-calendar/blob/master/json/{fileName}";

            // Проверяем существование локального файла
            if (File.Exists(localFilePath))
            {
                Console.WriteLine($"Local file already exists: {localFilePath}");
                return;
            }

            try
            {
                // Скачиваем файл из GitHub
                byte[] fileData;
                using (WebClient client = new WebClient())
                {
                    fileData = client.DownloadData(githubRawUrl);
                }

                // Пытаемся сохранить без повышенных прав
                try
                {
                    File.WriteAllBytes(localFilePath, fileData);
                    Console.WriteLine($"File saved successfully: {localFilePath}");
                }
                catch (UnauthorizedAccessException)
                {
                    Console.WriteLine("Administrator privileges required. Requesting elevation...");
                    SaveWithElevatedPrivileges(fileData, localFilePath);
                }
            }
            catch (WebException ex) when ((ex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotFound)
            {
                Console.WriteLine($"File not found at GitHub URL: {githubUrl}");
            }
        }

        private static void SaveWithElevatedPrivileges(byte[] fileData, string targetPath)
        {
            // Создаем временный файл в папке текущего пользователя
            string tempFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                Path.GetRandomFileName());

            try
            {
                File.WriteAllBytes(tempFile, fileData);

                // Настраиваем процесс для копирования с правами администратора
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/c copy /Y \"{tempFile}\" \"{targetPath}\" & exit 0",
                    Verb = "runas",
                    UseShellExecute = true,
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden
                };

                // Запускаем процесс с повышенными правами
                using (Process process = Process.Start(startInfo))
                {
                    process.WaitForExit();
                    if (process.ExitCode != 0)
                    {
                        throw new Exception($"Copy process failed with exit code: {process.ExitCode}");
                    }
                    Console.WriteLine($"File saved with administrator privileges: {targetPath}");
                }
            }
            catch (Win32Exception ex) when (ex.NativeErrorCode == 1223) // Отмена UAC
            {
                throw new UnauthorizedAccessException("User canceled elevation request", ex);
            }
            finally
            {
                // Удаляем временный файл
                if (File.Exists(tempFile))
                {
                    File.Delete(tempFile);
                }
            }
        }
    }
}