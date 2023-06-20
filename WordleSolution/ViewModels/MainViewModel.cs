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
        readonly DelegateCommand _StartCommand;

        string _btnTitle = "GameStart";

        public string ButtonTitle
        {
            get => _btnTitle;
            set => SetProperty(ref _btnTitle, value);
        }
        public ICommand StartCommand => _StartCommand;

        public MainViewModel(IEventAggregator eventAggregator, IWordleService wordleSvc)
        {
            _gameStatusChangedEventSubToken = eventAggregator.GetEvent<GameStatusChangedEvent>().Subscribe(onGameStatusChanged);
            _wordleSvc = wordleSvc;

            _StartCommand = new DelegateCommand(onStartCommandExecute);
        }

        public void Dispose()
        {
            _gameStatusChangedEventSubToken.Dispose();
        }

        private void onStartCommandExecute()
        {
            GameStatus gameStatus = _wordleSvc.GameStatus;
            switch (gameStatus)
            {
                case GameStatus.StandBy:
                case GameStatus.GameOver:
                    _wordleSvc.Start();
                    break;
                case GameStatus.Gaming:
                    _wordleSvc.Surrender();
                    break;
            }
        }
        private void onGameStatusChanged(GameStatusChangedEventArgs e)
        {
            GameStatus gameStatus = e.Status;
            switch(gameStatus)
            {
                case GameStatus.StandBy:
                case GameStatus.GameOver:
                    ButtonTitle = "GameStart";
                    break;
                case GameStatus.Gaming:
                    ButtonTitle = "Surrender";
                    break;
            }
        }
    }
}
