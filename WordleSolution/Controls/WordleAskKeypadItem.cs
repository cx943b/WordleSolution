using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Wordle.Controls
{
    internal class WordleAskKeypadItem : ContentControl
    {
        static WordleAskKeypadItem() => DefaultStyleKeyProperty.OverrideMetadata(typeof(WordleAskKeypadItem), new FrameworkPropertyMetadata(typeof(WordleAskKeypadItem)));
    }
}
