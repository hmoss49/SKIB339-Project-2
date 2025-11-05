namespace Game399.Shared.Runtime
{
    public interface IGameLog
    {
        void Info(string message);
        void Warning(string message);
        void Error(string message);
    }
}