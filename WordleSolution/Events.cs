using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wordle
{
    public class GameRemainCountChangedEvent : PubSubEvent<GameRemainCountChangedEventArgs> { }
    public class GameStatusChangedEvent : PubSubEvent<GameStatusChangedEventArgs> { }

    public class GameRemainCountChangedEventArgs: EventArgs
    {
        public int RemainCount { get; init; }

        public GameRemainCountChangedEventArgs(int remainCount)
        {
            if (remainCount < 0) throw new ArgumentOutOfRangeException(nameof(remainCount));

            RemainCount = remainCount;
        }
    }
    public class GameStatusChangedEventArgs : EventArgs
    {
        public GameStatus Status { get; init; }
        public AskResult AskResult { get; init; }

        public GameStatusChangedEventArgs(GameStatus status, AskResult askResult)
        {
            Status = status;
            AskResult = askResult;
        }
    }
}
