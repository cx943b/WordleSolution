using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WordleSolution.Models;

namespace Wordle.Views
{
    /// <summary>
    /// WordleLineView.xaml에 대한 상호 작용 논리
    /// </summary>
    internal partial class WordleLineView : UserControl, IWordleLine
    {
        public IEnumerable<WordleCharacterModel> AskModels
        {
            get
            {
                var vm = DataContext as IWordleLine;
                if(vm is null)
                    throw new NullReferenceException(nameof(vm));

                return vm.AskModels;
            }
        }

        public WordleLineView()
        {
            InitializeComponent();
        }

        public void PullCharacter()
        {
            var vm = DataContext as IWordleLine;
            if (vm is not null)
                vm.PullCharacter();
        }

        public void PushCharacter(char character)
        {
            var vm = DataContext as IWordleLine;
            if (vm is not null)
                vm.PushCharacter(character);
        }
    }
}
