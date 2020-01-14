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
            CheckAppPermissions();
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

        public bool CheckAppPermissions()
        {
            if ((int)Build.VERSION.SdkInt < 23)
            {
                return true;
            }

            if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.WriteExternalStorage) != (int)Permission.Granted 
                && ContextCompat.CheckSelfPermission(this, Manifest.Permission.ReadExternalStorage) != (int)Permission.Granted)
            {
                var permissions = new string[] { Manifest.Permission.ReadExternalStorage, Manifest.Permission.WriteExternalStorage };
                ActivityCompat.RequestPermissions(this, new []{Manifest.Permission.ReadExternalStorage}, 0);
                ActivityCompat.RequestPermissions(this, new []{Manifest.Permission.WriteExternalStorage}, 0);
                return false;
            }
            return true;

        }
    }
}