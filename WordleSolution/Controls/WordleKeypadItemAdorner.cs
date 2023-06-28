using System;
using System.Globalization;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace Wordle.Controls
{
    internal class WordleKeypadItemAdorner : Adorner
    {
        static readonly int _padding = 4;
        static readonly Size _renderSize = new Size(48, 48);
        static readonly Size _rectSize = new Size(_renderSize.Width - _padding * 2, _renderSize.Height - _padding * 2);
        
        static SolidColorBrush _background = new SolidColorBrush(Colors.HotPink) { Opacity = 0.7 };

        public static readonly DependencyProperty MousePositionProperty =
            DependencyProperty.Register("MousePosition", typeof(Point), typeof(WordleKeypadItemAdorner),
                new FrameworkPropertyMetadata(new Point(), FrameworkPropertyMetadataOptions.AffectsRender, onMousePositionPropertyChanged));

        Point _mousePos;



        public Point MousePosition
        {
            get { return (Point)GetValue(MousePositionProperty); }
            set { SetValue(MousePositionProperty, value); }
        }

        



        public Char Character { get; set; } = '_';

        public WordleKeypadItemAdorner(UIElement adornedElement) : base(adornedElement)
        {
            IsHitTestVisible = false;
        }

        protected override void OnRender(DrawingContext dc)
        {
            Point drawPos = new Point(_mousePos.X - _renderSize.Width, _mousePos.Y - _renderSize.Height);
            if(drawPos.X < 0)
                drawPos.X = 0;
            if(drawPos.Y < 0)
                drawPos.Y = 0;

            dc.DrawRoundedRectangle(_background, new Pen(Brushes.DimGray, 1), new Rect(new Point(drawPos.X + _padding, drawPos.Y + _padding), _rectSize), 4, 4);

            var ft = new FormattedText(Character.ToString(), CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Consolas"), 24, Brushes.Black, 96);
            dc.DrawText(ft, new Point(drawPos.X + _padding + (_rectSize.Width - ft.Width) / 2, drawPos.Y + _padding + (_rectSize.Height - ft.Height) / 2));
        }

        private static void onMousePositionPropertyChanged(DependencyObject depObj, DependencyPropertyChangedEventArgs e)
        {
            if (depObj is not WordleKeypadItemAdorner)
                return;

            ((WordleKeypadItemAdorner)depObj)._mousePos = (Point)e.NewValue;
        }
    }
}
