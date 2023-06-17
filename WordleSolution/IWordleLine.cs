using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordleSolution.Models;

namespace Wordle
{
    internal interface IWordleLine
    {
        IEnumerable<AskModel> AskModels { get; }

        void PushCharacter(char character);
        void PullCharacter();
    }
}
