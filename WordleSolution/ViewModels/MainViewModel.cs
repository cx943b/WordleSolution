using Microsoft.Extensions.DependencyInjection;
using Prism.Commands;
using Prism.Events;
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
    public class MainViewModel : BindableBase, IDisposable
    {
        readonly SubscriptionToken _gameStatusChangedEventSubToken;
        readonly IWordleService _wordleSvc;

        GameStatus _GameStatus = GameStatus.StandBy;

        public GameStatus GameStatus
        {
            get => _GameStatus;
            set => SetProperty(ref _GameStatus, value);
        }

        public MainViewModel(IEventAggregator eventAggregator, IWordleService wordleSvc)
        {
            _gameStatusChangedEventSubToken = eventAggregator.GetEvent<GameStatusChangedEvent>().Subscribe(onGameStatusChanged);
            _wordleSvc = wordleSvc;
        }

        public void Dispose()
        {
            _gameStatusChangedEventSubToken.Dispose();
        }

        public void Start() => _wordleSvc.Start();
        public void Surrender()
        {

        }

        private void onGameStatusChanged(GameStatusChangedEventArgs e)
        {

        }
    }
}
