using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wordle;
using Wordle.ViewModels;
using Wordle.Views;
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

        [TestMethod]
        public async Task AskWord()
        {
            IWordleService wordleSvc = new WordleService(_loggerFac.CreateLogger<WordleService>(), _config, null, null);
            
            bool isInit = await wordleSvc.InitializeAsync();
            Assert.IsTrue(isInit);

            bool isStarted = wordleSvc.Start();
            Assert.IsTrue(isStarted);

            WordleLineViewModel wordleLineVM = new WordleLineViewModel(_loggerFac.CreateLogger<WordleLineViewModel>());
            wordleLineVM.PushCharacter('t');
            wordleLineVM.PushCharacter('r');
            wordleLineVM.PushCharacter('u');
            wordleLineVM.PushCharacter('y');
            wordleLineVM.PushCharacter('t');
        }
    }
}
