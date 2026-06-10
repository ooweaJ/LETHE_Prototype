using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lethe.Dev
{
    public sealed class RunBuildState : MonoBehaviour
    {
        private const int MaxLevel = 5;

        private readonly Dictionary<string, int> activeMemoryLevels = new Dictionary<string, int>();
        private readonly Dictionary<string, int> echoLevels = new Dictionary<string, int>();
        private readonly HashSet<string> unlockedSynergies = new HashSet<string>();

        public event Action Changed;

        public IReadOnlyDictionary<string, int> ActiveMemoryLevels => activeMemoryLevels;
        public IReadOnlyDictionary<string, int> EchoLevels => echoLevels;
        public IReadOnlyCollection<string> UnlockedSynergies => unlockedSynergies;

        public int GetActiveMemoryLevel(string id)
        {
            return !string.IsNullOrEmpty(id) && activeMemoryLevels.TryGetValue(id, out var level) ? level : 0;
        }

        public int GetEchoLevel(string id)
        {
            return !string.IsNullOrEmpty(id) && echoLevels.TryGetValue(id, out var level) ? level : 0;
        }

        public void SetActiveMemoryLevel(string id, int level)
        {
            SetLevel(activeMemoryLevels, id, level);
        }

        public void RemoveActiveMemory(string id)
        {
            if (!string.IsNullOrEmpty(id) && activeMemoryLevels.Remove(id))
            {
                Changed?.Invoke();
            }
        }

        public int AddEchoLevel(string id, int amount, out int overflow)
        {
            overflow = 0;
            if (string.IsNullOrEmpty(id) || amount <= 0)
            {
                return GetEchoLevel(id);
            }

            var before = GetEchoLevel(id);
            var raw = before + amount;
            var clamped = Mathf.Clamp(raw, 0, MaxLevel);
            overflow = Mathf.Max(0, raw - MaxLevel);
            echoLevels[id] = clamped;
            Changed?.Invoke();
            return clamped;
        }

        public void SetEchoLevel(string id, int level)
        {
            SetLevel(echoLevels, id, level);
        }

        public void UnlockSynergy(string id)
        {
            if (!string.IsNullOrEmpty(id) && unlockedSynergies.Add(id))
            {
                Changed?.Invoke();
            }
        }

        public bool HasSynergy(string id)
        {
            return !string.IsNullOrEmpty(id) && unlockedSynergies.Contains(id);
        }

        public string FindHighestActiveMemory()
        {
            var bestId = string.Empty;
            var bestLevel = 0;
            foreach (var pair in activeMemoryLevels)
            {
                if (pair.Value > bestLevel)
                {
                    bestId = pair.Key;
                    bestLevel = pair.Value;
                }
            }

            return bestId;
        }

        private void SetLevel(IDictionary<string, int> target, string id, int level)
        {
            if (string.IsNullOrEmpty(id))
            {
                return;
            }

            var clamped = Mathf.Clamp(level, 0, MaxLevel);
            if (clamped == 0)
            {
                target.Remove(id);
            }
            else
            {
                target[id] = clamped;
            }

            Changed?.Invoke();
        }
    }
}
