using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wordle
{
    public class GameStatusChangedEvent : PubSubEvent<GameStatusChangedEventArgs> { }

    public class GameStatusChangedEventArgs : EventArgs
    {
        public GameStatus Status { get; init; }

        public GameStatusChangedEventArgs(GameStatus status)
        {
            Status = status;
        }
    }
}
