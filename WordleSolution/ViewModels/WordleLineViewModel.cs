using Microsoft.Extensions.Logging;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wordle.Models;

namespace Wordle.ViewModels
{
    internal class WordleLineViewModel : BindableBase
    {
        readonly ILogger<WordleLineViewModel> _logger;

        string _RegionName = "";

        IEnumerable<WordleCharacterModel> _CharModels = Enumerable.Empty<WordleCharacterModel>().ToArray();

        public string RegionName
        {
            get => _RegionName;
            set => SetProperty(ref _RegionName, value);
        }

        public IEnumerable<WordleCharacterModel> CharacterModels
        {
            get => _CharModels;
            internal set => SetProperty(ref _CharModels, value);
        }

        public WordleLineViewModel(ILogger<WordleLineViewModel> logger)
        {
            _logger = logger;
        }
    }
}
