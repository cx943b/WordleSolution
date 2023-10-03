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
    public class WordleLineViewParameterNames
    {
        public const string LineNumber = "LineNumber";
    }

    internal class WordleLineViewModel : BindableBase, INavigationAware
    {
        readonly ILogger<WordleLineViewModel> _logger;

        int _LineNumber;
        string _RegionName = "";

        IEnumerable<WordleCharacterModel> _CharModels = Enumerable.Empty<WordleCharacterModel>().ToArray();

        public string RegionName
        {
            get => _RegionName;
            set => SetProperty(ref _RegionName, value);
        }
        public int LineNumber
        {
            get => _LineNumber;
            set => SetProperty(ref _LineNumber, value);
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

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            //throw new NotImplementedException();
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            if(navigationContext.Parameters.TryGetValue(WordleLineViewParameterNames.LineNumber, out int lineNum))
                return lineNum == _LineNumber;
            
            return false;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            //throw new NotImplementedException();
        }
    }
}
