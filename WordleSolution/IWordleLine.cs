using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wordle.Models;

namespace Wordle
{
    internal interface IWordleLine
    {
        IEnumerable<WordleCharacterModel> AskModels { get; }

        void PushCharacter(char character);
        void PullCharacter();
    }
}
