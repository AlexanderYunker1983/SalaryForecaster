using Newtonsoft.Json;
using SalaryForecast.Core.Models;

namespace SalaryForecast.Core.Infrastructure.Impl
{
    public class JsonProvider : IJsonProvider
    {
        private IFileProvider fileProvider;

        public void SetFileProvider(IFileProvider fileProvider)
        {
            this.fileProvider = fileProvider;
        }

        public Holidays GetHolidays(int year)
        {
            var fileStream = fileProvider.GetJsonFile(year);
            if (fileStream == null)
            {
                return null;
            }

            var json = fileStream.ReadToEnd();
            var result = JsonConvert.DeserializeObject<Holidays>(json);
            fileStream.Dispose();
            return result;
        }
    }
}