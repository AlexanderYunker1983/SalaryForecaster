using MugenMvvmToolkit.Interfaces.Models;
using MugenMvvmToolkit.ViewModels;
using SalaryForecast.Core.Infrastructure;
using YLocalization;

namespace SalaryForecast.Core.ViewModels.StartViewModel
{
    public class SalaryForecasterStartViewModel : ViewModelBase, IHasDisplayName
    {
        private readonly ILocalizationManager localizationManager;
        private readonly IJsonProvider jsonProvider;
        public string DisplayName { get; set; }

        public SalaryForecasterStartViewModel(ILocalizationManager localizationManager, IJsonProvider jsonProvider)
        {
            this.localizationManager = localizationManager;
            this.jsonProvider = jsonProvider;
            DisplayName = $"{this.localizationManager.GetString("ProgramTitle")} v.{PlatformVariables.ProgramVersion}";

            var holidays = this.jsonProvider.GetHolidays(2020);
        }
    }
}