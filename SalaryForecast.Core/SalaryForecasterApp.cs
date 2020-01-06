using System;
using MugenMvvmToolkit;
using SalaryForecast.Core.ViewModels.StartViewModel;

namespace SalaryForecast.Core
{
    public class SalaryForecasterApp : MvvmApplication
    {
        protected override Type GetStartViewModelType()
        {
            return typeof(SalaryForecasterStartViewModel);
        }
    }
}