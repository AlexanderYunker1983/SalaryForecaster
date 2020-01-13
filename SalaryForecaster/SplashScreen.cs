using System.Reflection;
using MugenMvvmToolkit.Android.Infrastructure;
using MugenMvvmToolkit.Android.Views.Activities;

namespace SalaryForecaster
{
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