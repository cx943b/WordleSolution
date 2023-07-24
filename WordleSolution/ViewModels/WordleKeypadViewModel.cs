using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordleSolution.Models;

namespace Wordle.ViewModels
{
    internal class WordleKeypadViewModel : BindableBase
    {
        readonly IEnumerable<WordleCharacterModel> _CharModels;

        public IEnumerable<WordleCharacterModel> CharacterModels => _CharModels;

        public WordleKeypadViewModel(IAskModelService askModelSvc)
        {
            _CharModels = askModelSvc.CharacterModels;
        }
    }
}
