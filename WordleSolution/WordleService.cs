using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordleSolution
{
    public class WordleService : IWordleService
    {
        readonly Random _rand= new Random();
        readonly IConfiguration _config;
        readonly ILogger _logger;

        string[]? _words;
        string? _selectedWord;

        public WordleService(ILogger<WordleService> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        public async Task<bool> InitializeAsync()
        {
            Task<bool>[] initTasks = new Task<bool>[]
            {
                Task.Run(loadWords)
            };

            bool[] initResults = await Task.WhenAll(initTasks);
            return initResults.All(b => b);
        }
        public bool SelectWord()
        {
            if (_words is null)
            {
                _logger.Log(LogLevel.Error, "NotInitialized");
                return false;
            }

            int selectionIndex = _rand.Next(_words.Length);
            _selectedWord = _words[selectionIndex];

            return true;
        }

        private bool loadWords()
        {
            _words = _config.GetSection("Words").Get<string[]>();
            return _words != null;
        }
    }

    public interface IWordleService
    {
        Task<bool> InitializeAsync();
        bool SelectWord();
    }
}
