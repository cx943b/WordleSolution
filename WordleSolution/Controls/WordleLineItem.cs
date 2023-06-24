using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WordleSolution.Models;

namespace Wordle.Controls
{
    public class WordleLineItem : ContentControl
    {
        public bool IsDropTarget
        {
            get { return (bool)GetValue(IsDropTargetProperty); }
            set { SetValue(IsDropTargetProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsDropTarget.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsDropTargetProperty =
            DependencyProperty.Register("IsDropTarget", typeof(bool), typeof(WordleLineItem), new PropertyMetadata(false));



        static WordleLineItem() => DefaultStyleKeyProperty.OverrideMetadata(typeof(WordleLineItem), new FrameworkPropertyMetadata(typeof(WordleLineItem)));

        protected override void OnDragEnter(DragEventArgs e)
        {
            base.OnDragEnter(e);
            IsDropTarget = e.Data.GetDataPresent(typeof(AskModel));
        }
        protected override void OnDragLeave(DragEventArgs e)
        {
            base.OnDragLeave(e);
            IsDropTarget = false;
        }
    }
}
