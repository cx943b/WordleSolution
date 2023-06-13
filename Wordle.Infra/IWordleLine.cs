namespace Wordle.Infra
{
    public interface IWordleLine
    {
        void PushCharacter(char character);
        void PullCharacter();
    }
}
