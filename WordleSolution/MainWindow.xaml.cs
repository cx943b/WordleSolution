using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Wordle
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly IWordleService _wordleSvc;
        readonly ILogger<MainWindow> _logger;

        public MainWindow(ILogger<MainWindow> logger, IWordleService wordleSvc)
        {
            InitializeComponent();

            _wordleSvc = wordleSvc;
            _logger = logger;
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.Key >= Key.A && e.Key <= Key.Z)
                _wordleSvc.WriteChar(e.Key.ToString().First());
            else if (e.Key == Key.Enter)
                _wordleSvc.AskWord();
            else if (e.Key == Key.Back)
                _wordleSvc.EraseChar();

            _logger.Log(LogLevel.Information, e.Key.ToString());
        }
    }
}
