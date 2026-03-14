using System.IO;
using Newtonsoft.Json;
using SalaryForecast.Core.Models;

namespace SalaryForecast.Core.Infrastructure.Impl
{
    public class JsonProvider : IJsonProvider
    {
        private IFileProvider? _fileProvider;

        public void SetFileProvider(IFileProvider fileProvider)
        {
            _fileProvider = fileProvider;
        }

        public Holidays? GetHolidays(int year)
        {
            if (_fileProvider == null) return null;

            var fileStream = _fileProvider.GetJsonFile(year);
            if (fileStream == null) return null;

            using (fileStream)
            {
                try
                {
                    var json = fileStream.ReadToEnd();
                    return JsonConvert.DeserializeObject<Holidays>(json);
                }
                catch (JsonException)
                {
                    return null;
                }
                catch (IOException)
                {
                    return null;
                }
            }
        }
    }
}
