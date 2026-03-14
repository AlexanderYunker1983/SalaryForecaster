using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using SalaryForecast.Core.Infrastructure;

namespace SalaryForecast.Desktop.Infrastructure.Impl
{
    public class CalendarImportService : ICalendarImportService
    {
        private readonly IFileProvider _fileProvider;

        public CalendarImportService(IFileProvider fileProvider)
        {
            _fileProvider = fileProvider;
        }

        public async Task<bool> TryImportCalendarsAsync()
        {
            var sourceDirectory = await SelectSourceDirectoryAsync();
            if (string.IsNullOrWhiteSpace(sourceDirectory) || !Directory.Exists(sourceDirectory))
            {
                return false;
            }

            try
            {
                var sourceFiles = Directory.GetFiles(sourceDirectory, "consultant*.json", SearchOption.TopDirectoryOnly);
                if (sourceFiles.Length == 0) return false;

                var targetDirectory = _fileProvider.GetJsonDirectory();
                Directory.CreateDirectory(targetDirectory);

                var copiedAny = false;
                foreach (var sourceFile in sourceFiles)
                {
                    var targetFile = Path.Combine(targetDirectory, Path.GetFileName(sourceFile));
                    if (string.Equals(sourceFile, targetFile, StringComparison.OrdinalIgnoreCase))
                    {
                        copiedAny = true;
                        continue;
                    }

                    File.Copy(sourceFile, targetFile, overwrite: true);
                    copiedAny = true;
                }

                return copiedAny;
            }
            catch (IOException)
            {
                return false;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
        }

        private static async Task<string?> SelectSourceDirectoryAsync()
        {
            if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop ||
                desktop.MainWindow?.StorageProvider == null ||
                !desktop.MainWindow.StorageProvider.CanPickFolder)
            {
                return null;
            }

            var folders = await desktop.MainWindow.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
            {
                Title = "Выберите каталог с производственными календарями",
                AllowMultiple = false
            });

            return folders.FirstOrDefault()?.TryGetLocalPath();
        }
    }
}
