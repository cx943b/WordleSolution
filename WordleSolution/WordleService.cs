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

        readonly GameStatusChangedEvent _gameStatusChangedEvent;

        GameStatus _GameStatus = GameStatus.StandBy;
        IRegion? _wordleLinesRegion;

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

            _wordleLinesRegion = _regionMgr.Regions[WellknownRegionNames.WordleLinesRegion];
            if (_wordleLinesRegion is null)
            {
                _logger.Log(LogLevel.Error, $"NullRef: {_wordleLinesRegion}");
                return false;
            }

            bool[] initResults = await Task.WhenAll(initTasks);
            return initResults.All(b => b);
        }

        public bool Start()
        {
            if (_wordleLinesRegion is null)
            {
                _logger.Log(LogLevel.Error, $"NullRef: {_wordleLinesRegion}");
                return false;
            }

            _wordleLinesRegion.RemoveAll();

            try
            {
                selectWord();
                addWordleLine();
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
            _gameStatusChangedEvent.Publish(new GameStatusChangedEventArgs(GameStatus.GameOver, AskResult.Fail));
        }
        public bool WriteChar(char ch)
        {
            if (_wordleLinesRegion is null)
            {
                _logger.Log(LogLevel.Error, $"NullRef: {_wordleLinesRegion}");
                return false;
            }

            var activeWordleLineView = _wordleLinesRegion.ActiveViews.FirstOrDefault() as IWordleLine;
            if(activeWordleLineView is not null)
            {
                activeWordleLineView?.PushCharacter(ch);
                return true;
            }
            else
            {
                _logger.Log(LogLevel.Error, $"NullRef: {activeWordleLineView}");
                return false;
            }
        }
        public bool EraseChar()
        {
            if (_wordleLinesRegion is null)
            {
                _logger.Log(LogLevel.Error, $"NullRef: {_wordleLinesRegion}");
                return false;
            }

            var activeWordleLineView = _wordleLinesRegion.ActiveViews.FirstOrDefault() as IWordleLine;
            if (activeWordleLineView is not null)
            {
                activeWordleLineView?.PullCharacter();
                return true;
            }
            else
            {
                _logger.Log(LogLevel.Error, $"NullRef: {activeWordleLineView}");
                return false;
            }
        }

        /// <exception cref="NullReferenceException"></exception>
        private AskResult askWord()
        {
            if (_wordleLinesRegion is null)
                throw new NullReferenceException(nameof(_wordleLinesRegion));
            if (_words is null)
                throw new NullReferenceException(nameof(_words));
            if (_selectedWord is null)
                throw new NullReferenceException(nameof(_selectedWord));

            IWordleLine? wordleLine = _wordleLinesRegion.ActiveViews.LastOrDefault() as IWordleLine;
            if (wordleLine is null)
                throw new NullReferenceException(nameof(wordleLine));

            IEnumerable<WordleCharacterModel> askModels = wordleLine.AskModels;

            foreach ((char currentCh, WordleCharacterModel currentAsk) in _selectedWord.Zip(askModels))
            {
                if (currentCh == currentAsk.Character)
                {
                    currentAsk.IsCurrected = true;
                }
                else if (_selectedWord.IndexOf(currentAsk.Character, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    currentAsk.IsExisted = true;
                }
            }

            if (askModels.Any(am => !am.IsCurrected))
            {
                if (_wordleLinesRegion.Views.Count() == MaxAskCount)
                    return AskResult.CountOver;


                return AskResult.WaitNext;
            }

            return AskResult.Currect;
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

        /// <exception cref="NullReferenceException"></exception>
        private void addWordleLine()
        {
            if (_wordleLinesRegion is null)
                throw new NullReferenceException(nameof(_wordleLinesRegion));

            var newWordleLine = createWordleLine();
            NavigationParameters navParams = new NavigationParameters
            {
                { WordleLineViewModel.NavParamName, _wordleLinesRegion.Views.Count() }
            };

            _wordleLinesRegion.Add(newWordleLine);
            _regionMgr.RequestNavigate(WellknownRegionNames.WordleLinesRegion, nameof(WordleLineView), navParams);

            _wordleLinesRegion.Activate(newWordleLine);
        }

        /// <exception cref="NullReferenceException"></exception>
        private WordleLineView createWordleLine()
        {
            if (_wordleLinesRegion is null)
                throw new NullReferenceException(nameof(_wordleLinesRegion));
            if (_selectedWord is null)
                throw new NullReferenceException(nameof(_selectedWord));

            var view =_containerProv.Resolve<WordleLineView>();
            var vm = (WordleLineViewModel)view.DataContext;

            vm.AskModels = Enumerable.Range(0, _selectedWord.Length).Select(i => new WordleLineCharacterModel()).ToArray();
            vm.LineIndex = _wordleLinesRegion.Views.Count();

            return view;
        }
    }
}
