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
using Wordle.Controls;
using WordleSolution.Models;

namespace Wordle.Views
{
    /// <summary>
    /// MainView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainView : UserControl
    {
        WordleAskKeypadItemAdorner _adorner;

        public MainView()
        {
            InitializeComponent();
        }

        private void DockPanel_DragEnter(object sender, DragEventArgs e)
        {
            if (_adorner is null)
            {
                _adorner = new WordleAskKeypadItemAdorner(this);
                _adorner.Character = ((AskModel)e.Data.GetData(typeof(AskModel))).Character;
                AdornerLayer adoLayer = AdornerLayer.GetAdornerLayer(this);
                adoLayer.Add(_adorner);

                Point mousePos = e.GetPosition(this);
                _adorner.Point = mousePos;
                _adorner.InvalidateVisual();
            }
        }

        private void DockPanel_DragLeave(object sender, DragEventArgs e)
        {
            if (_adorner is not null)
            {
                AdornerLayer adoLayer = AdornerLayer.GetAdornerLayer(this);
                adoLayer.Remove(_adorner);

                _adorner = null;
            }
        }

        private void DockPanel_DragOver(object sender, DragEventArgs e)
        {
            Point mousePos = e.GetPosition(this);
            _adorner.Point = mousePos;
            _adorner.InvalidateVisual();
        }
    }
}
