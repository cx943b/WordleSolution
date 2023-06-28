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
        WordleKeypadItemAdorner? _adorner;

        public MainView()
        {
            InitializeComponent();
        }

        protected override void OnDragEnter(DragEventArgs e)
        {
            base.OnDragEnter(e);

            if (!e.Data.GetDataPresent(typeof(WordleCharacterModel)))
                return;

            WordleCharacterModel? askModel = e.Data.GetData(typeof(WordleCharacterModel)) as WordleCharacterModel;
            if (askModel != null)
                showDragAdorner(askModel, e.GetPosition(this));
        }
        protected override void OnDragLeave(DragEventArgs e)
        {
            base.OnDragLeave(e);

            hideDragAdorner();
        }
        protected override void OnDragOver(DragEventArgs e)
        {
            base.OnDragOver(e);

            if (_adorner is null)
                return;

            Point mousePos = e.GetPosition(this);
            _adorner.MousePosition = mousePos;
        }
        protected override void OnDrop(DragEventArgs e)
        {
            base.OnDrop(e);

            if (!e.Data.GetDataPresent(typeof(WordleCharacterModel)))
                return;

            hideDragAdorner();
        }

        private void showDragAdorner(WordleCharacterModel askModel, Point mousePos)
        {
            if (_adorner is null)
            {
                _adorner = new WordleKeypadItemAdorner(this);
                _adorner.Character = askModel.Character;

                AdornerLayer adoLayer = AdornerLayer.GetAdornerLayer(this);
                adoLayer.Add(_adorner);

                _adorner.MousePosition = mousePos;
            }
        }
        private void hideDragAdorner()
        {
            if (_adorner is not null)
            {
                AdornerLayer adoLayer = AdornerLayer.GetAdornerLayer(this);
                adoLayer.Remove(_adorner);

                _adorner = null;
            }
        }
    }
}
