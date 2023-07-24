using System.Threading.Tasks;

namespace Wordle
{
    public interface IWordleService
    {
        GameStatus GameStatus { get; }

        bool AskWord();
        Task<bool> InitializeAsync();
        bool Start();
        void Surrender();
    }
}
