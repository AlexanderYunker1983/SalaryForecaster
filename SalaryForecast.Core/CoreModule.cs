using System;
using MugenMvvmToolkit;
using MugenMvvmToolkit.Binding;
using MugenMvvmToolkit.Interfaces;
using MugenMvvmToolkit.Interfaces.Models;
using MugenMvvmToolkit.Models.IoC;
using SalaryForecast.Core.Infrastructure;
using SalaryForecast.Core.Infrastructure.Impl;
using YLocalization;
using YMugenExtensions;

namespace SalaryForecast.Core
{
    public class CoreModule : IModule
    {
        public bool Load(IModuleContext context)
        {
            var resourceResolver = BindingServiceProvider.ResourceResolver;
            resourceResolver.AddType("DateTimeOffset", typeof(DateTimeOffset));

            var iocContainer = context.IocContainer;
            if (!iocContainer.CanResolve<ILocalizationManager>()) iocContainer.Bind<ILocalizationManager, MugenLocalizationManager>(DependencyLifecycle.SingleInstance);
            if (!iocContainer.CanResolve<IJsonProvider>()) iocContainer.Bind<IJsonProvider, JsonProvider>(DependencyLifecycle.SingleInstance);
            if (!iocContainer.CanResolve<ICalendarProvider>()) iocContainer.Bind<ICalendarProvider, CalendarProvider>(DependencyLifecycle.SingleInstance);
            if (!iocContainer.CanResolve<IDbService>()) iocContainer.Bind<IDbService, DbService>(DependencyLifecycle.SingleInstance);
            if (!iocContainer.CanResolve<ISalaryProvider>()) iocContainer.Bind<ISalaryProvider, SalaryProvider>(DependencyLifecycle.SingleInstance);
            return true;
        }

        public void Unload(IModuleContext context)
        {
        }

        public int Priority => ApplicationSettings.ModulePriorityDefault;
    }
}