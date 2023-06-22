using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using WordleSolution.Models;

namespace Wordle.Controls
{
    internal class WordleAskKeypadItem : ContentControl
    {
        public const int DragStartLength = 10;

        WordleAskKeypadItemAdorner _adorner;
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

                //_adorner = new WordleAskKeypadItemAdorner(this);
                //_adorner.Character = ((AskModel)this.DataContext).Character;
                //AdornerLayer adoLayer = AdornerLayer.GetAdornerLayer(this);
                //adoLayer.Add(_adorner);

                DragDrop.DoDragDrop(this, this.DataContext, DragDropEffects.Copy);

                //if(_adorner is not null)
                //    adoLayer.Remove(_adorner);
            }
        }

        //protected override void OnDrop(DragEventArgs e)
        //{
        //    base.OnDrop(e);

        //    if(_adorner is not null)
        //    {
        //        AdornerLayer adoLayer = AdornerLayer.GetAdornerLayer(this);
        //        adoLayer.Remove(_adorner);

        //        _adorner = null;
        //    }
        //}
        //protected override void OnDragEnter(DragEventArgs e)
        //{
        //    base.OnDragEnter(e);

        //    if(_adorner is null)
        //    {
        //        _adorner = new WordleAskKeypadItemAdorner(this);
        //        _adorner.Character = ((AskModel)e.Data.GetData(typeof(AskModel))).Character;
        //        AdornerLayer adoLayer = AdornerLayer.GetAdornerLayer(this);
        //        adoLayer.Add(_adorner);

        //        Point mousePos = e.GetPosition(this);
        //        _adorner.Point = mousePos;
        //        _adorner.InvalidateVisual();
        //    }
        //}
        //protected override void OnDragLeave(DragEventArgs e)
        //{
        //    base.OnDragLeave(e);

        //    if(_adorner is not null)
        //    {
        //        AdornerLayer adoLayer = AdornerLayer.GetAdornerLayer(this);
        //        adoLayer.Remove(_adorner);

        //        _adorner = null;
        //    }
        //}
        //protected override void OnDragOver(DragEventArgs e)
        //{
        //    base.OnDragOver(e);


        //    Point mousePos = e.GetPosition(this);
        //    _adorner.Point  = mousePos;
        //    _adorner.InvalidateVisual();

        //}
    }

    internal class WordleAskKeypadItemAdorner : Adorner
    {
        public Char Character { get; set; } = '_';
        public Point Point { get; set; }

        public WordleAskKeypadItemAdorner(UIElement adornedElement) : base(adornedElement)
        {
            IsHitTestVisible = false;
        }

        protected override void OnRender(DrawingContext dc)
        {
            Point startPoint = new Point(Point.X - 35, Point.Y - 40);
            Size adorSize = new Size(30, 40);

            SolidColorBrush brush = new SolidColorBrush(Colors.HotPink) { Opacity = 0.4 };
            dc.DrawRoundedRectangle(brush, new Pen(Brushes.LightGray, 1), new Rect(startPoint, adorSize), 4, 4);

            var ft = new FormattedText(Character.ToString(), CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Consolas"), 14, Brushes.Black, 96);
            dc.DrawText(ft, new Point(startPoint.X + (adorSize.Width - ft.Width) / 2, startPoint.Y + (adorSize.Height - ft.Height) / 2));
        }
    }
}
