using MugenMvvmToolkit;
using MugenMvvmToolkit.Android.Infrastructure;
using MugenMvvmToolkit.Interfaces;
using SalaryForecast.Core;

namespace SalaryForecaster
{
    public class MugenSetup : AndroidBootstrapperBase
    {
        protected override IMvvmApplication CreateApplication()
        {
            return new SalaryForecasterApp();
        }

        protected override IIocContainer CreateIocContainer()
        {
            return new AutofacContainer();
        }
    }
}