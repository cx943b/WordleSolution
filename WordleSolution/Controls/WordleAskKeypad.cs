using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Wordle.Controls
{
    internal class WordleAskKeypad : ItemsControl
    {
        static WordleAskKeypad() => DefaultStyleKeyProperty.OverrideMetadata(typeof(WordleAskKeypad), new FrameworkPropertyMetadata(typeof(WordleAskKeypad)));
        protected override DependencyObject GetContainerForItemOverride() => new WordleAskKeypadItem();
    }
}
