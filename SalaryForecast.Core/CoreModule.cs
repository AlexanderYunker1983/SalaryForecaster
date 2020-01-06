using System;
using MugenMvvmToolkit;
using MugenMvvmToolkit.Binding;
using MugenMvvmToolkit.Interfaces;
using MugenMvvmToolkit.Interfaces.Models;
using MugenMvvmToolkit.Models.IoC;
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
            if (!iocContainer.CanResolve<ILocalizationManager>())
            {
                iocContainer.Bind<ILocalizationManager, MugenLocalizationManager>(DependencyLifecycle.SingleInstance);
            }

            return true;
        }

        public void Unload(IModuleContext context)
        {
        }

        public int Priority => ApplicationSettings.ModulePriorityDefault;
    }
}