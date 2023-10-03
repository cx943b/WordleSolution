using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Wordle.Models;

namespace Wordle.Models
{
    internal class WordleLineModel : BindableBase
    {
        readonly ObservableCollection<WordleCharacterModel> _lstCharModel = new ObservableCollection<WordleCharacterModel>();
        readonly CollectionViewSource _charCVS = new CollectionViewSource();

        public ICollectionView CharacterModels => _charCVS.View;

        public WordleLineModel()
        {
            _charCVS.Source = _lstCharModel;
        }

        internal void SetCharacterModels(IEnumerable<WordleCharacterModel> charModels) => _lstCharModel.AddRange(charModels);
    }
}
