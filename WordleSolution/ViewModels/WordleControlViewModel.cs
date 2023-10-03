using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Wordle.ViewModels
{
    internal class WordleControlViewModel : BindableBase, IDisposable
    {
        readonly SubscriptionToken _gameStatusChangedEventSubToken;
        readonly IWordleService _wordleSvc;

        readonly DelegateCommand _StartCommand;
        readonly DelegateCommand _SurrenderCommand;
        readonly DelegateCommand _SubmitCommand;

        GameStatus _GameStatus;

        public GameStatus GameStatus
        {
            get => _GameStatus;
            set => SetProperty(ref _GameStatus, value);
        }
        public ICommand StartCommand => _StartCommand;
        public ICommand SurrenderCommand => _SurrenderCommand;
        public ICommand SubmitCommand => _SubmitCommand;

        public WordleControlViewModel(IEventAggregator eventAggregator, IWordleService wordleSvc)
        {
            _gameStatusChangedEventSubToken = eventAggregator.GetEvent<GameStatusChangedEvent>().Subscribe(onGameStatusChanged);
            _wordleSvc = wordleSvc;

            _StartCommand = new DelegateCommand(onStartCommandExecute, () => GameStatus == GameStatus.StandBy || GameStatus == GameStatus.GameOver).ObservesProperty(() => GameStatus);
            _SurrenderCommand = new DelegateCommand(onSurrenderCommandExecute, () => GameStatus == GameStatus.Gaming).ObservesProperty(() => GameStatus);
            _SubmitCommand = new DelegateCommand(onSubmitCommandExecute, () => GameStatus == GameStatus.Gaming).ObservesProperty(() => GameStatus);
        }

        public void Dispose()
        {
            _gameStatusChangedEventSubToken.Dispose();
        }

        private void onStartCommandExecute() => _wordleSvc.Start();
        private void onSurrenderCommandExecute() => _wordleSvc.Surrender();
        private void onSubmitCommandExecute() => _wordleSvc.AskWord();
        private void onGameStatusChanged(GameStatusChangedEventArgs e)
        {
            GameStatus = e.Status;
        }
    }
}
