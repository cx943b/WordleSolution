using Prism.Unity;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using Serilog;
using Wordle.Views;
using Wordle.Controls;
using Wordle.ViewModels;

namespace Wordle
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override Window CreateShell() => Container.Resolve<MainWindow>();
        protected override async void InitializeShell(Window shell)
        {
            base.InitializeShell(shell);

            IRegionManager regionMgr = Container.Resolve<IRegionManager>();
            regionMgr.RegisterViewWithRegion<MainView>(WellknownRegionNames.MainViewRegion);
            regionMgr.RegisterViewWithRegion<WordleLineView>(WellknownRegionNames.WordleLineViewRegion);
            regionMgr.RegisterViewWithRegion<WordleKeypadView>(WellknownRegionNames.WordleKeypadViewRegion);
            regionMgr.RegisterViewWithRegion<WordleControlView>(WellknownRegionNames.WordleControlViewRegion);
            regionMgr.RegisterViewWithRegion<WordleStateView>(WellknownRegionNames.WordleStateViewRegion);

            IWordleService wordleSvc = Container.Resolve<IWordleService>();
            await wordleSvc.InitializeAsync();
        }
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IConfiguration>(prov =>
            {
                return new ConfigurationBuilder()
                    .SetBasePath(Environment.CurrentDirectory)
                    .AddJsonFile("appsettings.json")
                    .Build();
            });
            containerRegistry.RegisterSingleton<ILoggerFactory>(prov =>
            {
                var config = prov.Resolve<IConfiguration>();

                return LoggerFactory.Create(builder =>
                {
                    var logger = new LoggerConfiguration()
                        .ReadFrom.Configuration(config)
                        .CreateLogger();

                    builder
                        .ClearProviders()
                        .AddSerilog(logger)
                        .AddDebug();
                });
            });
            containerRegistry.Register(typeof(ILogger<>), typeof(Logger<>));

            containerRegistry.RegisterSingleton<IAskModelService, AskModelService>();
            containerRegistry.RegisterSingleton<IWordleService, WordleService>();
        }

        protected override void ConfigureRegionAdapterMappings(RegionAdapterMappings regionAdapterMappings)
        {
            base.ConfigureRegionAdapterMappings(regionAdapterMappings);

            regionAdapterMappings.RegisterMapping<WordleLine>(Container.Resolve<WordleLineItemsRegionAdapter>());
            regionAdapterMappings.RegisterMapping<WordleLines>(Container.Resolve<WordleLinesRegionAdapter>());
        }
    }
}