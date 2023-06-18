using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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
    public enum AskResult { Currect, WaitNext, CountOver }
    public class WordleService : IWordleService
    {
        const int MaxAskCount = 5;

        readonly Random _rand= new Random();
        readonly IConfiguration _config;
        private readonly IRegionManager _regionMgr;
        readonly IContainerProvider _containerProv;
        readonly ILogger _logger;
        IRegion _wordleLinesRegion;

        string[]? _words;
        string? _selectedWord;


        public WordleService(ILogger<WordleService> logger, IConfiguration config, IRegionManager regionMgr, IContainerProvider containerProv)
        {
            _logger = logger;
            _config = config;
            _regionMgr = regionMgr;
            _containerProv = containerProv;
        }

        public async Task<bool> InitializeAsync()
        {
            Task<bool>[] initTasks = new Task<bool>[]
            {
                Task.Run(loadWords)
            };

            _wordleLinesRegion = _regionMgr.Regions[WellknownRegionNames.WordleLinesRegion];

            bool[] initResults = await Task.WhenAll(initTasks);
            return initResults.All(b => b);
        }

        public bool Start()
        {
            _wordleLinesRegion.RemoveAll();

            bool isWordSelected = selectWord();
            if (!isWordSelected)
            {
                _logger.Log(LogLevel.Error, "CantSelectSord");
                return false;
            }

            addWordleLine();
            return true;
        }
        public void AskWord()
        {
            AskResult askResult = askWord();

            if (askResult == AskResult.WaitNext)
            {
                if(_wordleLinesRegion.Views.Count() >= MaxAskCount)
                {

                }
                else
                {
                    addWordleLine();
                }
            }
                


        }

        public void WriteChar(char ch)
        {
            var activeWordleLineView = _wordleLinesRegion.ActiveViews.FirstOrDefault() as IWordleLine;
            activeWordleLineView?.PushCharacter(ch);
        }
        public void EraseChar()
        {
            var activeWordleLineView = _wordleLinesRegion.ActiveViews.FirstOrDefault() as IWordleLine;
            activeWordleLineView?.PullCharacter();
        }

        private AskResult askWord()
        {
            if (_words is null)
                throw new NullReferenceException(nameof(_words));
            if (_selectedWord is null)
                throw new NullReferenceException(nameof(_selectedWord));

            IWordleLine? wordleLine = _wordleLinesRegion.ActiveViews.LastOrDefault() as IWordleLine;
            if (wordleLine is null)
                throw new NullReferenceException(nameof(wordleLine));

            IEnumerable<AskModel> askModels = wordleLine.AskModels;

            foreach ((char currentCh, AskModel currentAsk) in _selectedWord.Zip(askModels))
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
            _words = _config.GetSection("Words").Get<string[]>();
            return _words != null;
        }
        private bool selectWord()
        {
            _selectedWord = null;

            if (_words is null)
            {
                _logger.Log(LogLevel.Error, "NotInitialized");
                return false;
            }

            int selectionIndex = _rand.Next(_words.Length);
            _selectedWord = _words[selectionIndex].ToUpper();

            return true;
        }
        private void addWordleLine()
        {
            var newWordleLine = createWordleLine();
            NavigationParameters navParams = new NavigationParameters
            {
                { WordleLineViewModel.NavParamName, _wordleLinesRegion.Views.Count() }
            };

            _wordleLinesRegion.Add(newWordleLine);
            _regionMgr.RequestNavigate(WellknownRegionNames.WordleLinesRegion, nameof(WordleLineView), navParams);

            _wordleLinesRegion.Activate(newWordleLine);
        }
        private WordleLineView createWordleLine()
        {
            var view =_containerProv.Resolve<WordleLineView>();
            var vm = (WordleLineViewModel)view.DataContext;

            vm.AskModels = Enumerable.Range(0, _selectedWord!.Length).Select(i => new AskModel()).ToArray();
            vm.LineIndex = _wordleLinesRegion.Views.Count();

            return view;
        }
    }

    public interface IWordleService
    {
        void AskWord();
        void EraseChar();
        Task<bool> InitializeAsync();
        bool Start();
        void WriteChar(char ch);
    }
}
