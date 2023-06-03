using System;
using MugenMvvmToolkit;
using SalaryForecast.Core.ViewModels.StartViewModel;

namespace SalaryForecast.Core
{
    public class SalaryForecasterApp : MvvmApplication
    {
        public override Type GetStartViewModelType()
        {
            return typeof(SalaryForecasterStartViewModel);
        }
    }
}