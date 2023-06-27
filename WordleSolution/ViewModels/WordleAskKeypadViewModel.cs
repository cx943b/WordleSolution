using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordleSolution.Models;

namespace Wordle.ViewModels
{
    internal class WordleAskKeypadViewModel : BindableBase
    {
        readonly IEnumerable<WordleCharacterModel> _AskModels;

        public IEnumerable<WordleCharacterModel> AskModels => _AskModels;

        public WordleAskKeypadViewModel(IAskModelService askModelSvc)
        {
            _AskModels = askModelSvc.AskModels;
        }
    }
}
