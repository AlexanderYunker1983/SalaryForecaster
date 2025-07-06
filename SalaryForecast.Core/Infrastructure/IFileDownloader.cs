namespace SalaryForecast.Core.Infrastructure
{
    public interface IFileDownloader
    {
        void EnsureConsultantFile(int year, string directoryPath);
    }
}