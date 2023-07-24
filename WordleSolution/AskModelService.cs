using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordleSolution.Models;

namespace Wordle
{
    internal class AskModelService : IAskModelService
    {
        readonly WordleCharacterModel[] _CharModels;

        public IEnumerable<WordleCharacterModel> CharacterModels => _CharModels;

        public AskModelService()
        {
            int nA = 'A';
            int modelCount = ('Z' - nA) + 1;

            _CharModels = Enumerable.Range(nA, modelCount).Select(i => new WordleCharacterModel { Character = (char)i }).ToArray();
        }

        public void Clear()
        {
            foreach(var askModel in _CharModels)
            {
                askModel.IsCurrected = false;
                askModel.IsExisted = false;
                askModel.IsInputTarget = false;
            }
        }
    }

    internal interface IAskModelService
    {
        IEnumerable<WordleCharacterModel> CharacterModels { get; }

        void Clear();
    }
}
