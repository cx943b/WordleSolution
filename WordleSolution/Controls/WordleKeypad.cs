using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Wordle.Controls
{
    internal class WordleKeypad : ItemsControl
    {
        static WordleKeypad() => DefaultStyleKeyProperty.OverrideMetadata(typeof(WordleKeypad), new FrameworkPropertyMetadata(typeof(WordleKeypad)));
        protected override DependencyObject GetContainerForItemOverride() => new WordleKeypadItem();
    }
}
