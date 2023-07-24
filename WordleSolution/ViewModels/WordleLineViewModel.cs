using Microsoft.Extensions.Logging;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordleSolution.Models;

namespace Wordle.ViewModels
{
    internal class WordleLineViewModel : BindableBase
    {
        readonly ILogger<WordleLineViewModel> _logger;

        IEnumerable<WordleCharacterModel> _CharModels = Enumerable.Empty<WordleCharacterModel>().ToArray();

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
