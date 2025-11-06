using Game399.Shared.Diagnostics;

namespace Game.Runtime
{
    public class UnityGameLogger : IGameLog
    {
        public void Info(string message)
        {
            Debug.Log(message);
        }

        public void Warning(string message)
        {
            Debug.LogWarning(message);
        }

        public void Error(string message)
        {
            Debug.LogError(message);
        }
    }
}