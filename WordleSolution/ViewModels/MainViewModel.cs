using Microsoft.Extensions.DependencyInjection;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WordleSolution.Models;

namespace Wordle.ViewModels
{
    public class MainViewModel : BindableBase
    {
        readonly IWordleService _wordleSvc;

        public MainViewModel(IWordleService wordleSvc)
        {
            _wordleSvc = wordleSvc;
        }

        public void Start()
        {
            bool isStarted = _wordleSvc.Start();
            if(!isStarted)
            {
                return;
            }
        }
    }
}
