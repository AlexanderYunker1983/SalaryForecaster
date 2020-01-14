using Android.App;
using Android.Content.PM;
using Android.Views;
using MugenMvvmToolkit.Android.Views.Activities;

namespace SalaryForecaster.Views.StartView
{
    [Activity(Label = "SalaryForecaster", WindowSoftInputMode = SoftInput.StateHidden,
        ConfigurationChanges = ConfigChanges.Density | ConfigChanges.FontScale | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden |
                               ConfigChanges.LayoutDirection | ConfigChanges.Locale | ConfigChanges.Mcc | ConfigChanges.Mnc |
                               ConfigChanges.Navigation | ConfigChanges.Orientation | ConfigChanges.ScreenLayout |
                               ConfigChanges.ScreenSize | ConfigChanges.SmallestScreenSize | ConfigChanges.Touchscreen | ConfigChanges.UiMode)]
    public class SalaryForecasterStartView : MvvmActivity
    {
        public SalaryForecasterStartView() : base(Resource.Layout.activity_main)
        {
        }
    }
}