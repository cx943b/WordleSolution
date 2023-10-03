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
using Wordle.Models;
using Wordle.ViewModels;
using Wordle.Views;
using Wordle.Models;

namespace Wordle
{

    public class WordleService : IWordleService
    {
        const int MaxAskCount = 5;

        readonly Random _rand = new Random();
        readonly IConfiguration _config;
        readonly IRegionManager _regionMgr;
        readonly IContainerProvider _containerProv;
        readonly ILogger _logger;

        readonly IList<IEnumerable<WordleLineCharacterModel>> _lstCharModels = new List<IEnumerable<WordleLineCharacterModel>>();
        readonly GameStatusChangedEvent _gameStatusChangedEvent;
        readonly GameRemainCountChangedEvent _gameRemainCountChangedEvent;

        IRegion? _wordleLinesRegion;
        GameStatus _GameStatus = GameStatus.StandBy;

        int _remainCount;
        string[]? _words;
        string? _selectedWord;

        public GameStatus GameStatus => _GameStatus;


        public WordleService(ILogger<WordleService> logger, IConfiguration config, IRegionManager regionMgr, IEventAggregator eventAggregator, IContainerProvider containerProv)
        {
            _logger = logger;
            _config = config;
            _regionMgr = regionMgr;
            _containerProv = containerProv;

            _gameRemainCountChangedEvent = eventAggregator.GetEvent<GameRemainCountChangedEvent>();
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
            if (initResults.All(b => b))
            {
                _wordleLinesRegion = _regionMgr.Regions[WellknownRegionNames.WordleLinesRegion];
                if (_wordleLinesRegion == null)
                    throw new NullReferenceException(nameof(_wordleLinesRegion));

                //printWord("Wellcome!");
                return true;
            }

            return false;
        }

        public bool Start()
        {
            if (_wordleLinesRegion is null)
                throw new NullReferenceException(nameof(_wordleLinesRegion));

            _wordleLinesRegion.RemoveAll();
            _remainCount = 5;

            selectWord();
            printWord(String.Concat(Enumerable.Range(0, _selectedWord!.Length).Select(i => ' ')), false);

            _gameRemainCountChangedEvent.Publish(new GameRemainCountChangedEventArgs(_remainCount));
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
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);
                return false;
            }

            switch (askResult)
            {
                case AskResult.CountOver:   notifyGameStatus(GameStatus.GameOver, askResult); break;
                case AskResult.Currect:     notifyGameStatus(GameStatus.GameOver, askResult); break;
                case AskResult.WaitNext:    notifyGameStatus(GameStatus.Gaming, askResult); break;
            }

            notifyGameRemainCount(_remainCount);
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
            //if (!_lstCharModels.Any())
            //    throw new InvalidOperationException($"EmptyList: {_lstCharModels}");

            var targetCharacterModels = _lstCharModels.Last();
            if (targetCharacterModels.Any(m => m.Character == ' '))
                return AskResult.WaitNext;

            StringBuilder sbNextPrint = new StringBuilder();

            foreach ((WordleLineCharacterModel charModel, char selectedChar) in targetCharacterModels.Zip(_selectedWord))
            {
                if (charModel.Character == selectedChar)
                {
                    // CurrectChar
                    charModel.IsCurrected = true;
                    sbNextPrint.Append(charModel.Character);
                }
                else if (_selectedWord.Contains(charModel.Character))
                {
                    // ExistChar
                    charModel.IsExisted = true;
                    sbNextPrint.Append(' ');
                }
                else
                {
                    // Nothing
                    charModel.IsExepted = false;
                    sbNextPrint.Append(' ');
                }
            }

            --_remainCount;

            bool isAllCurrected = !targetCharacterModels.Any(m => !m.IsCurrected);
            if(isAllCurrected)
            {
                return AskResult.Currect;
            }
            else if(_remainCount <= 0)
            {
                printWord("End!");
                return AskResult.CountOver;
            }
            else
            {
                string currectedChars = sbNextPrint.ToString();
                var lineCharModels = currectedChars.Select(currectedChar =>
                {
                    bool isPrint = currectedChar != ' ';
                    return new WordleLineCharacterModel { Character = currectedChar, IsPrinted = isPrint, IsCurrected = isPrint };
                }).ToArray(); // Caution

                printWord(lineCharModels);
                return AskResult.WaitNext;
            }
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
        private void printWord(string word,bool isNotice = true)
        {
            ArgumentNullException.ThrowIfNull(word, nameof(word));

            if (isNotice)
                _wordleLinesRegion.RemoveAll();

            var charModels = word.Select(ch => new WordleLineCharacterModel { Character = ch, IsPrinted = isNotice }).ToArray();
            printWord(charModels);
        }
        private void printWord(IEnumerable<WordleLineCharacterModel> wordleLineModels)
        {
            ArgumentNullException.ThrowIfNull(wordleLineModels, nameof(wordleLineModels));
            if (!wordleLineModels.Any())
                throw new ArgumentOutOfRangeException($"EmptyArray: {wordleLineModels}");
            if (_wordleLinesRegion is null)
                throw new NullReferenceException(nameof(_wordleLinesRegion));

            // ChangeToReadOnly-LastWordleLine
            if (_lstCharModels.Any())
            {
                foreach (var charModel in _lstCharModels.Last())
                {
                    charModel.IsPrinted = true;
                }
            }

            _lstCharModels.Add(wordleLineModels);

            WordleLineModel lineModel = new WordleLineModel();
            lineModel.SetCharacterModels(wordleLineModels);

            _wordleLinesRegion.Add(lineModel);
        }

        private void notifyGameStatus(GameStatus gameStatus, AskResult askResult) => _gameStatusChangedEvent.Publish(new GameStatusChangedEventArgs(gameStatus, askResult));
        private void notifyGameRemainCount(int remainCount) => _gameRemainCountChangedEvent.Publish(new GameRemainCountChangedEventArgs(remainCount));
    }
}
