using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Wordle.Controls
{
    public class WordleLine : ItemsControl
    {
        static WordleLine()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WordleLine), new FrameworkPropertyMetadata(typeof(WordleLine)));
        }

        protected override DependencyObject GetContainerForItemOverride() => new WordleLineItem();
    }
}