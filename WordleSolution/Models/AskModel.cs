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
        char _Character;
        bool _IsExisted;
        bool _IsCurrected;

        public char Character
        {
            get => _Character;
            set => SetProperty(ref _Character, value);
        }
        public bool IsExisted
        {
            get => IsExisted;
            set => SetProperty(ref _IsExisted, value);
        }
        public bool IsCurrected
        {
            get => IsCurrected;
            set => SetProperty(ref _IsCurrected, value);
        }
    }
}
