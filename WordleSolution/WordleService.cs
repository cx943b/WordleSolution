using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Prism.Events;
using Prism.Ioc;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xaml;
using Wordle.ViewModels;
using Wordle.Views;
using WordleSolution.Models;

namespace Wordle
{
    
    public class WordleService : IWordleService
    {
        const int MaxAskCount = 5;

        readonly Random _rand= new Random();
        readonly IConfiguration _config;
        readonly IRegionManager _regionMgr;
        readonly IContainerProvider _containerProv;
        readonly ILogger _logger;

        IRegion _wordleLineItemsRegion;

        readonly GameStatusChangedEvent _gameStatusChangedEvent;

        GameStatus _GameStatus = GameStatus.StandBy;
        

        string[]? _words;
        string? _selectedWord;

        public GameStatus GameStatus => _GameStatus;


        public WordleService(ILogger<WordleService> logger, IConfiguration config, IRegionManager regionMgr, IEventAggregator eventAggregator, IContainerProvider containerProv)
        {
            _logger = logger;
            _config = config;
            _regionMgr = regionMgr;
            _containerProv = containerProv;

            
            _gameStatusChangedEvent = eventAggregator.GetEvent<GameStatusChangedEvent>();
        }

        public async Task<bool> InitializeAsync()
        {
            Task<bool>[] initTasks = new Task<bool>[]
            {
                Task.Run(loadWords)
                // Etc.
            };

            bool[] initResults = await Task.WhenAll(initTasks);
            if(initResults.All(b => b))
            {
                _wordleLineItemsRegion = _regionMgr.Regions[WellknownRegionNames.WordleLineItemsRegion];

                if (_wordleLineItemsRegion == null)
                    throw new NullReferenceException(nameof(_wordleLineItemsRegion));

                printWord("Wellcome!");
                return true;
            }

            return false;
        }

        public bool Start()
        {
            try
            {
                selectWord();
                printWord(String.Concat(Enumerable.Range(0, _selectedWord!.Length).Select(i => ' ').ToArray()), false);
            }
            catch(Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);
                return false;
            }
            
            _gameStatusChangedEvent.Publish(new GameStatusChangedEventArgs(GameStatus.Gaming, AskResult.WaitNext));

            return true;
        }
        public bool AskWord()
        {
            AskResult askResult = AskResult.Fail;

            try
            {
                askResult = askWord();
            }
            catch(Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);
                return false;
            }

            switch(askResult)
            {
                case AskResult.CountOver:
                    _gameStatusChangedEvent.Publish(new GameStatusChangedEventArgs(GameStatus.GameOver, askResult));
                    break;
                case AskResult.Currect:
                    _gameStatusChangedEvent.Publish(new GameStatusChangedEventArgs(GameStatus.GameOver, askResult));
                    break;
                case AskResult.WaitNext:
                    _gameStatusChangedEvent.Publish(new GameStatusChangedEventArgs(GameStatus.Gaming, askResult));
                    break;
            }

            return askResult is not AskResult.Fail;
        }
        public void Surrender()
        {
            printWord("YouDead");

            _gameStatusChangedEvent.Publish(new GameStatusChangedEventArgs(GameStatus.StandBy, AskResult.Fail));
        }

        /// <exception cref="NullReferenceException"></exception>
        private AskResult askWord()
        {
            if (_words is null)
                throw new NullReferenceException(nameof(_words));
            if (_selectedWord is null)
                throw new NullReferenceException(nameof(_selectedWord));

            AskResult result = AskResult.Currect;

            // ToDo
            foreach((WordleLineCharacterModel lineCharModel, char selectedChar) in _wordleLineItemsRegion.Views.Cast<WordleLineCharacterModel>().Zip(_selectedWord))
            {
                if(lineCharModel.Character == selectedChar)
                {
                    // CurrectChar
                    
                }
                else if(_selectedWord.Contains(lineCharModel.Character))
                {
                    // ExistChar
                    result = AskResult.WaitNext;
                }
                else
                {
                    // Nothing
                    result = AskResult.WaitNext;
                }
            }

            return result;
        }
        private bool loadWords()
        {
            _words = _config.GetSection("Words")?.Get<string[]>();
            return _words != null;
        }

        /// <exception cref="NullReferenceException"></exception>
        private void selectWord()
        {
            _selectedWord = null;

            if (_words is null)
                throw new NullReferenceException(nameof(_words));

            int selectionIndex = _rand.Next(_words.Length);
            _selectedWord = _words[selectionIndex].ToUpper();
        }
        private void printWord(string word, bool isReadonly = true)
        {
            if (String.IsNullOrEmpty(word))
                throw new NullReferenceException(nameof(word));

            _wordleLineItemsRegion.RemoveAll();

            foreach (var c in word.Select(ch => new WordleLineCharacterModel { Character = ch, IsPrinted = isReadonly }))
            {
                _wordleLineItemsRegion.Add(c);
            }
        }
    }
}
