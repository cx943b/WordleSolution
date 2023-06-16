﻿using System;
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
        public bool IsTargetLine
        {
            get { return (bool)GetValue(IsTargetLineProperty); }
            set { SetValue(IsTargetLineProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsTargetLine.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsTargetLineProperty =
            DependencyProperty.Register("IsTargetLine", typeof(bool), typeof(WordleLine), new PropertyMetadata(false));



        static WordleLine()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WordleLine), new FrameworkPropertyMetadata(typeof(WordleLine)));
        }

        protected override DependencyObject GetContainerForItemOverride() => new WordleLineItem();
    }
}