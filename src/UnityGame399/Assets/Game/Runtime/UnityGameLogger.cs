using UnityEngine;

namespace Game399.Shared.Runtime
{
    public class UnityGameLogger : IGameLog
    {
        public void Info(string message)
        {
            Debug.Log($"[INFO] {message}");
        }

        public void Warning(string message)
        {
            Debug.LogWarning($"[WARNING] {message}");
        }

        public void Error(string message)
        {
            Debug.LogError($"[ERROR] {message}");
        }
    }
}