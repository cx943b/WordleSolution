using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Wordle.Models;

namespace Wordle.Controls
{
    internal class WordleKeypadItem : ContentControl
    {
        public static readonly DependencyProperty IsCurrectedProperty = DependencyProperty.Register("IsCurrected", typeof(bool), typeof(WordleKeypadItem), new PropertyMetadata(false));
        public static readonly DependencyProperty IsExistedProperty = DependencyProperty.Register("IsExisted", typeof(bool), typeof(WordleKeypadItem), new PropertyMetadata(false));

        public const int DragStartLength = 10;
        Point _dragStartMousePos;

        public bool IsExisted
        {
            get { return (bool)GetValue(IsExistedProperty); }
            set { SetValue(IsExistedProperty, value); }
        }
        public bool IsCurrected
        {
            get { return (bool)GetValue(IsCurrectedProperty); }
            set { SetValue(IsCurrectedProperty, value); }
        }

        static WordleKeypadItem() => DefaultStyleKeyProperty.OverrideMetadata(typeof(WordleKeypadItem), new FrameworkPropertyMetadata(typeof(WordleKeypadItem)));

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
