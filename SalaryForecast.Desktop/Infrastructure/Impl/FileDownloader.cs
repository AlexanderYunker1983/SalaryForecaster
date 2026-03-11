using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using SalaryForecast.Core.Infrastructure;

namespace SalaryForecast.Desktop.Infrastructure.Impl
{
    public class FileDownloader : IFileDownloader
    {
        private static readonly HttpClient HttpClient = new();

        public async Task EnsureConsultantFileAsync(int year, string directoryPath, CancellationToken cancellationToken = default)
        {
            var fileName = $"consultant{year}.json";
            var localFilePath = Path.Combine(directoryPath, fileName);
            if (File.Exists(localFilePath)) return;

            var githubRawUrl = $"https://raw.githubusercontent.com/d10xa/holidays-calendar/master/json/{fileName}";
            using var response = await HttpClient.GetAsync(githubRawUrl, cancellationToken);
            if (response.StatusCode == HttpStatusCode.NotFound) return;

            response.EnsureSuccessStatusCode();

            Directory.CreateDirectory(directoryPath);
            await using var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
            await using var fileStream = File.Create(localFilePath);
            await responseStream.CopyToAsync(fileStream, cancellationToken);
        }
    }
}
