using MugenMvvmToolkit.Interfaces.Models;
using MugenMvvmToolkit.ViewModels;
using YLocalization;

namespace SalaryForecast.Core.ViewModels.StartViewModel
{
    public class SalaryForecasterStartViewModel : ViewModelBase, IHasDisplayName
    {
        private readonly ILocalizationManager localizationManager;
        public string DisplayName { get; set; }

        public SalaryForecasterStartViewModel(ILocalizationManager localizationManager)
        {
            this.localizationManager = localizationManager;
            DisplayName = $"{this.localizationManager.GetString("ProgramTitle")} v.{PlatformVariables.ProgramVersion}";
        }
    }
}