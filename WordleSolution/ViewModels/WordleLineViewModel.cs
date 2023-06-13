using Microsoft.Extensions.Logging;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wordle.Infra;
using WordleSolution.Models;

namespace Wordle.ViewModels
{
    internal class WordleLineViewModel : BindableBase, IWordleLine
    {
        readonly ILogger<WordleLineViewModel> _logger;
        readonly IWordleService _wordleSvc;

        readonly AskModel[] _askModels;

        public IEnumerable<AskModel> AskModels => _askModels;

        public WordleLineViewModel(ILogger<WordleLineViewModel> logger ,IWordleService wordleSvc)
        {
            _logger = logger;
            _wordleSvc = wordleSvc;

            if(_wordleSvc.SelectedWordLength == 0)
            {
                string exMsg = "NotSelectedWord";

                _logger.Log(LogLevel.Error, exMsg);
                throw new NullReferenceException(exMsg);
            }

            _askModels = Enumerable.Range(0, _wordleSvc.SelectedWordLength).Select(i => new AskModel()).ToArray();
        }

        public void PullCharacter()
        {
            AskModel? targetModel = _askModels.LastOrDefault (am => am.Character != '_');
            if (targetModel is not null)
                targetModel.Character = ' ';
        }

        public void PushCharacter(char character)
        {
            if(!Char.IsLetter(character))
            {
                _logger.Log(LogLevel.Warning, "PushCharacter was not Letter");
                return;
            }

            AskModel? targetModel = _askModels.FirstOrDefault(am => am.Character == '_');
            if(targetModel is not null)
                targetModel.Character = character;
        }
    }
}
