using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordleSolution;
using WordleSolution.Models;

namespace WordleTests
{
    [TestClass]
    public class WordleServiceTest
    {
        ILoggerFactory _loggerFac;
        IConfiguration _config;

        [TestInitialize]
        public void Setup()
        {
            _config = new ConfigurationBuilder()
                    .SetBasePath(Environment.CurrentDirectory)
                    .AddJsonFile("appsettings.json")
                    .Build();

            _loggerFac = LoggerFactory.Create(builder =>
            {
                var logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(_config)
                    .CreateLogger();

                builder
                    .ClearProviders()
                    .AddSerilog(logger)
                    .AddDebug();
            });
        }

        public async Task AskWord()
        {
            IWordleService wordleSvc = new WordleService(_loggerFac.CreateLogger<WordleService>(), _config);
            bool isInit = await wordleSvc.InitializeAsync();

            Assert.IsTrue(isInit);

            bool isSelected = wordleSvc.SelectWord();
            Assert.IsTrue(isSelected);
        }
    }
}
