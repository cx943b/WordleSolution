using System.Threading.Tasks;

namespace Wordle
{
    public interface IWordleService
    {
        GameStatus GameStatus { get; }

        bool AskWord();
        Task<bool> InitializeAsync();
        void InitWordleLine();
        bool Start();
        void Surrender();
    }
}
