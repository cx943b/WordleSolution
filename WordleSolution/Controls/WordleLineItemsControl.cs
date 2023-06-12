using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Wordle.Controls
{
    public class WordleLineItemsControl : ItemsControl
    {
        static WordleLineItemsControl()
        {
            DefaultStyleKeyProperty.AddOwner(typeof(WordleLineItemsControl));
        }
    }
}
