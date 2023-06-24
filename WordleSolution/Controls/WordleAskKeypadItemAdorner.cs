using System;
using System.Globalization;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace Wordle.Controls
{
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
