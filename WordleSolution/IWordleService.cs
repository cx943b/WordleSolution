using System.Threading.Tasks;

namespace Wordle
{
    public interface IWordleService
    {
        bool AskWord();
        bool EraseChar();
        Task<bool> InitializeAsync();
        bool Start();
        void Surrender();
        bool WriteChar(char ch);
    }
}
