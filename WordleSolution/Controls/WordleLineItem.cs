using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Wordle.Controls
{
    public class WordleLineItem : ContentControl
    {
        public bool IsInputTarget
        {
            get { return (bool)GetValue(IsInputTargetProperty); }
            set { SetValue(IsInputTargetProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsInputTarget.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsInputTargetProperty =
            DependencyProperty.Register("IsInputTarget", typeof(bool), typeof(WordleLineItem), new UIPropertyMetadata(false));
    }
}
