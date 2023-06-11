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

namespace WordleSolution
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override async void InitializeShell(Window shell)
        {
            base.InitializeShell(shell);

            var wordleSvc = Container.Resolve<IWordleService>();
            await wordleSvc.InitializeAsync();
        }
        protected override Window CreateShell() => Container.Resolve<MainWindow>();

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


            containerRegistry.RegisterSingleton<IWordleService, WordleService>();
        }
    }
}
