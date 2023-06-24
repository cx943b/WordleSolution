using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WordleSolution.Models;

namespace Wordle.Controls
{
    internal class WordleAskKeypadItem : ContentControl
    {
        public const int DragStartLength = 10;
        Point _dragStartMousePos;

        static WordleAskKeypadItem() => DefaultStyleKeyProperty.OverrideMetadata(typeof(WordleAskKeypadItem), new FrameworkPropertyMetadata(typeof(WordleAskKeypadItem)));

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            _dragStartMousePos = PointToScreen(e.GetPosition(this));

            CaptureMouse();
        }
        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);

            if (IsMouseCaptured)
                ReleaseMouseCapture();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (!IsMouseCaptured)
                return;
            
            Point mousePos = PointToScreen(e.GetPosition(this));
            var moveLength = (mousePos - _dragStartMousePos).Length;

            if(moveLength > DragStartLength)
            {
                ReleaseMouseCapture();

                DragDrop.DoDragDrop(this, this.DataContext, DragDropEffects.Copy);
            }
        }
    }
}
