using System.Reflection;
using Android;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using MugenMvvmToolkit.Android.Infrastructure;
using MugenMvvmToolkit.Android.Views.Activities;

namespace SalaryForecaster
{
    [Activity(MainLauncher = true, NoHistory = true, Label = "@string/ApplicationName")]
    public class SplashScreen : SplashScreenActivityBase
    {
        protected override string GetApplicationName()
        {
            return Resources.GetString(Resource.String.ApplicationName);
        }

        protected override string GetVersion()
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            return version.ToString();
        }

        protected override string GetFooter()
        {
            return string.Empty;
        }

        protected override AndroidBootstrapperBase CreateBootstrapper()
        {
            return new MugenSetup();
        }
    }
}