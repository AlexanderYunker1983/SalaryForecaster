using System;
using System.IO;
using System.Reflection;
using Android.App;
using MugenMvvmToolkit;
using MugenMvvmToolkit.Interfaces;
using MugenMvvmToolkit.Interfaces.Models;
using MugenMvvmToolkit.Models.IoC;
using SalaryForecast.Core;
using SalaryForecast.Core.Infrastructure;
using SalaryForecaster.Infrastructure.Impl;
using Exception = Java.Lang.Exception;

namespace SalaryForecaster
{
    public class AndroidModule : IModule
    {
        private readonly object[] _menuStructure = {
            new MenuWithSubItems("Settings",
                new[]
                {
                    MainMenuItems.SalarySettings,
                }),
            new MenuWithSubItems("AdditionalPays",
                new[]
                {
                    MainMenuItems.AdditionalPaysTable,
                }),
        };

        public bool Load(IModuleContext context)
        {
            var iocContainer = context.IocContainer;
            if (!iocContainer.CanResolve<IFileProvider>()) iocContainer.Bind<IFileProvider, FileProvider>(DependencyLifecycle.SingleInstance);
            if (!iocContainer.CanResolve<ISettingsManager>()) iocContainer.Bind<ISettingsManager, SettingsManager>(DependencyLifecycle.SingleInstance);

            PlatformVariables.MenuStructure = _menuStructure;
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            PlatformVariables.ProgramVersion = version.Build == 0 ? $"{version.Major}.{version.Minor}" : $"{version.Major}.{version.Minor}.{version.Build}-Developer Version";
            var fileProvider = iocContainer.Get<IFileProvider>();
            var basePath = fileProvider.GetDbFilePath();

            for (var index = 2011; index <= 2020; index++)
            {
                var manualName = Path.Combine(basePath, $"consultant{index}.json");
                try
                {
                    if (!File.Exists(manualName))
                        using (var br = new BinaryReader(Application.Context.Assets.Open($"consultant{index}.json")))
                        {
                            using (var bw = new BinaryWriter(new FileStream(manualName, FileMode.Create)))
                            {
                                var buffer = new byte[2048];
                                int length;
                                while ((length = br.Read(buffer, 0, buffer.Length)) > 0) bw.Write(buffer, 0, length);
                            }
                        }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }

            return true;
        }

        public void Unload(IModuleContext context)
        {
        }

        public int Priority => ApplicationSettings.ModulePriorityDefault;
    }
}