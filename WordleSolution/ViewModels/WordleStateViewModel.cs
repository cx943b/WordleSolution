using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wordle.ViewModels
{
    internal class WordleStateViewModel : BindableBase,IDisposable
    {
        int _RemainCount = 0;
        GameStatus _GameStatus = GameStatus.StandBy;
        AskResult _AskResult = AskResult.WaitNext;

        SubscriptionToken[] _eventTokens;

        public int RemainCount
        {
            get => _RemainCount;
            set => SetProperty(ref _RemainCount, value);
        }
        public GameStatus GameStatus
        {
            get => _GameStatus;
            set => SetProperty(ref _GameStatus, value);
        }
        public AskResult AskResult
        {
            get => _AskResult;
            set => SetProperty(ref _AskResult, value);
        }


        public WordleStateViewModel(IEventAggregator eventAggregator)
        {
            _eventTokens = new SubscriptionToken[]
                {
                    eventAggregator.GetEvent<GameStatusChangedEvent>().Subscribe(onGameStatusChanged),
                    eventAggregator.GetEvent<GameRemainCountChangedEvent>().Subscribe(onRemainCountChanged)
                };
        }

        public void Dispose()
        {
            if(_eventTokens is not null)
            {
                foreach (var token in _eventTokens)
                    token.Dispose();
            }
        }

        private void onGameStatusChanged(GameStatusChangedEventArgs e)
        {
            GameStatus = e.Status;
            AskResult = e.AskResult;
        }
        private void onRemainCountChanged(GameRemainCountChangedEventArgs e)
        {
            RemainCount = e.RemainCount;
        }
    }
}
