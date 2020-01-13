using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;

namespace SalaryForecaster
{
    [Application(LargeHeap = true, Label = "@string/ApplicationName")]
    public class SalaryForecasterApplication : Application
    {
        public override void OnCreate()
        {
            RegisterActivityLifecycleCallbacks(new SetCommonSettingsToActivity());
        }

        private class SetCommonSettingsToActivity : Java.Lang.Object, IActivityLifecycleCallbacks
        {
            public void OnActivityCreated(Activity activity, Bundle savedInstanceState)
            {
                activity.RequestedOrientation = ScreenOrientation.SensorLandscape;
                activity.Window.AddFlags(WindowManagerFlags.KeepScreenOn);
                activity.Window.AddFlags(WindowManagerFlags.Fullscreen);
            }

            public void OnActivityDestroyed(Activity activity)
            {
            }

            public void OnActivityPaused(Activity activity)
            {
            }

            public void OnActivityResumed(Activity activity)
            {
            }

            public void OnActivitySaveInstanceState(Activity activity, Bundle outState)
            {
            }

            public void OnActivityStarted(Activity activity)
            {
            }

            public void OnActivityStopped(Activity activity)
            {
            }
        }
    }
}