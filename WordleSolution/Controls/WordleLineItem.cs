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
        public static readonly DependencyProperty IsCurrectedProperty = DependencyProperty.Register("IsCurrected", typeof(bool), typeof(WordleLineItem), new PropertyMetadata(false));
        public static readonly DependencyProperty IsExistedProperty = DependencyProperty.Register("IsExisted", typeof(bool), typeof(WordleLineItem), new PropertyMetadata(false));
        public static readonly DependencyProperty IsExceptedProperty = DependencyProperty.Register("IsExcepted", typeof(bool), typeof(WordleLineItem), new PropertyMetadata(false));
        public static readonly DependencyProperty IsDropTargetProperty = DependencyProperty.Register("IsDropTarget", typeof(bool), typeof(WordleLineItem), new PropertyMetadata(false));

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
        public bool IsExcepted
        {
            get { return (bool)GetValue(IsExceptedProperty); }
            set { SetValue(IsExceptedProperty, value); }
        }
        public bool IsDropTarget
        {
            get { return (bool)GetValue(IsDropTargetProperty); }
            set { SetValue(IsDropTargetProperty, value); }
        }


        static WordleLineItem() => DefaultStyleKeyProperty.OverrideMetadata(typeof(WordleLineItem), new FrameworkPropertyMetadata(typeof(WordleLineItem)));
    }
}
