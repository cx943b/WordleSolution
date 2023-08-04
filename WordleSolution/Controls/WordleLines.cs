using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Wordle.Controls
{
    internal class WordleLines : ItemsControl
    {
        static WordleLines() => DefaultStyleKeyProperty.OverrideMetadata(typeof(WordleLines), new FrameworkPropertyMetadata(typeof(WordleLines)));
    }
}
