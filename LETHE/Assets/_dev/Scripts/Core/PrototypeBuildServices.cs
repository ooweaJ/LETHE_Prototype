using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lethe.Dev
{
    public sealed class MemoryInventory
    {
        private const int MaxLevel = 5;
        private readonly Dictionary<string, int> levels = new Dictionary<string, int>();

        public IReadOnlyDictionary<string, int> Levels => levels;
        public int Count => levels.Count;

        public int GetLevel(string id)
        {
            return !string.IsNullOrEmpty(id) && levels.TryGetValue(id, out var level) ? level : 0;
        }

        public int SetLevel(string id, int level)
        {
            if (string.IsNullOrEmpty(id))
            {
                return 0;
            }

            var clamped = Mathf.Clamp(level, 0, MaxLevel);
            if (clamped <= 0)
            {
                levels.Remove(id);
            }
            else
            {
                levels[id] = clamped;
            }

            return clamped;
        }

        public int AddLevel(string id, int amount)
        {
            return SetLevel(id, GetLevel(id) + Mathf.Max(0, amount));
        }

        public bool Remove(string id)
        {
            return !string.IsNullOrEmpty(id) && levels.Remove(id);
        }

        public string FindHighestLevelMemory()
        {
            var bestId = string.Empty;
            var bestLevel = -1;
            foreach (var pair in levels)
            {
                if (pair.Value > bestLevel)
                {
                    bestId = pair.Key;
                    bestLevel = pair.Value;
                }
            }

            return bestId;
        }

        public void Clear()
        {
            levels.Clear();
        }
    }

    public sealed class EchoInventory
    {
        private const int MaxLevel = 5;
        private readonly Dictionary<string, int> levels = new Dictionary<string, int>();

        public IReadOnlyDictionary<string, int> Levels => levels;
        public int Count => levels.Count;

        public int GetLevel(string id)
        {
            return !string.IsNullOrEmpty(id) && levels.TryGetValue(id, out var level) ? level : 0;
        }

        public int SetLevel(string id, int level)
        {
            if (string.IsNullOrEmpty(id))
            {
                return 0;
            }

            var clamped = Mathf.Clamp(level, 0, MaxLevel);
            if (clamped <= 0)
            {
                levels.Remove(id);
            }
            else
            {
                levels[id] = clamped;
            }

            return clamped;
        }

        public int AddLevel(string id, int amount, out int overflow)
        {
            var before = GetLevel(id);
            var raw = before + Mathf.Max(0, amount);
            overflow = Mathf.Max(0, raw - MaxLevel);
            return SetLevel(id, raw);
        }

        public void Clear()
        {
            levels.Clear();
        }
    }

    public sealed class ForgetService
    {
        public string SelectHighestMemory(MemoryInventory memories)
        {
            return memories != null ? memories.FindHighestLevelMemory() : string.Empty;
        }

        public bool ForgetHighest(MemoryInventory memories, EchoInventory echoes, Func<string, string> matchingEcho, out string memoryId, out string echoId, out int lostLevel, out int echoLevel, out int overflow)
        {
            memoryId = SelectHighestMemory(memories);
            echoId = string.Empty;
            lostLevel = 0;
            echoLevel = 0;
            overflow = 0;

            if (memories == null || echoes == null || string.IsNullOrEmpty(memoryId))
            {
                return false;
            }

            lostLevel = memories.GetLevel(memoryId);
            if (lostLevel <= 0)
            {
                return false;
            }

            echoId = matchingEcho != null ? matchingEcho(memoryId) : string.Empty;
            memories.Remove(memoryId);
            echoLevel = echoes.AddLevel(echoId, lostLevel, out overflow);
            return true;
        }
    }

    public sealed class ResonanceService
    {
        public int GetReacquiredLevel(int baseLevel, int echoLevel)
        {
            return Mathf.Clamp(baseLevel + Mathf.FloorToInt(Mathf.Max(0, echoLevel) * 0.5f), 1, 5);
        }
    }

    public sealed class UltimateEchoService
    {
        private readonly HashSet<string> unlocked = new HashSet<string>();

        public IReadOnlyCollection<string> Unlocked => unlocked;

        public bool IsUnlocked(string id)
        {
            return !string.IsNullOrEmpty(id) && unlocked.Contains(id);
        }

        public bool Refresh(EchoInventory echoes, IReadOnlyList<PrototypeSynergySpec> synergies)
        {
            var changed = false;
            if (echoes == null || synergies == null)
            {
                return false;
            }

            for (var index = 0; index < synergies.Count; index += 1)
            {
                var synergy = synergies[index];
                if (string.IsNullOrEmpty(synergy.Id) || synergy.RequiredEchoIds == null)
                {
                    continue;
                }

                var ready = true;
                for (var echoIndex = 0; echoIndex < synergy.RequiredEchoIds.Length; echoIndex += 1)
                {
                    if (echoes.GetLevel(synergy.RequiredEchoIds[echoIndex]) < synergy.RequiredLevel)
                    {
                        ready = false;
                        break;
                    }
                }

                if (ready && unlocked.Add(synergy.Id))
                {
                    changed = true;
                }
            }

            return changed;
        }

        public void Clear()
        {
            unlocked.Clear();
        }
    }

    public sealed class RewardService
    {
        private int cursor;

        public string NextMemory(IReadOnlyList<PrototypeMemorySpec> memories)
        {
            if (memories == null || memories.Count == 0)
            {
                return string.Empty;
            }

            var id = memories[cursor % memories.Count].Id;
            cursor += 1;
            return id;
        }

        public void Reset()
        {
            cursor = 0;
        }
    }

    public sealed class DebugStateInjector
    {
        public void SetAllMemories(MemoryInventory memories, IReadOnlyList<PrototypeMemorySpec> specs, int level)
        {
            if (memories == null || specs == null)
            {
                return;
            }

            for (var index = 0; index < specs.Count; index += 1)
            {
                memories.SetLevel(specs[index].Id, level);
            }
        }

        public void SetAllEchoes(EchoInventory echoes, IReadOnlyList<PrototypeEchoSpec> specs, int level)
        {
            if (echoes == null || specs == null)
            {
                return;
            }

            for (var index = 0; index < specs.Count; index += 1)
            {
                echoes.SetLevel(specs[index].Id, level);
            }
        }
    }
}
