using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordleSolution.Models
{
    public class AskModel : BindableBase
    {
        char _Character = '_';
        bool _IsInputTarget;
        bool _IsExisted;
        bool _IsCurrected;

        public bool IsInputTarget
        {
            get => _IsInputTarget;
            set => SetProperty(ref _IsInputTarget, value);
        }
        public char Character
        {
            get => _Character;
            set => SetProperty(ref _Character, value);
        }
        public bool IsExisted
        {
            get => _IsExisted;
            set => SetProperty(ref _IsExisted, value);
        }
        public bool IsCurrected
        {
            get => _IsCurrected;
            set => SetProperty(ref _IsCurrected, value);
        }
    }

    public class WordleLineAskModel : AskModel
    {

    }
}
