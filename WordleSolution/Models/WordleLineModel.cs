using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wordle.Models;

namespace Wordle.Models
{
    internal class WordleLineModel : BindableBase
    {
        string _RegionName = "";

        IEnumerable<WordleLineCharacterModel> _CharModels = Enumerable.Empty<WordleLineCharacterModel>().ToArray();

        public string RegionName
        {
            get => _RegionName;
            set => SetProperty(ref _RegionName, value);
        }

        public IEnumerable<WordleLineCharacterModel> CharacterModels
        {
            get => _CharModels;
            internal set => SetProperty(ref _CharModels, value);
        }

    }
}
