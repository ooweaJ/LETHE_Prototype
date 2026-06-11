using UnityEngine;

namespace Lethe.Dev
{
    public enum PrototypeEffectKind
    {
        Kalmuri,
        Blood,
        Execution,
        Homing,
        Shockwave,
        TimeStop,
        AshenGuard,
        Brand
    }

    public sealed class PrototypeMemorySpec
    {
        public PrototypeMemorySpec(string id, string displayName, string echoId, PrototypeEffectKind kind, Color color, string activeNote, string echoNote)
        {
            Id = id;
            DisplayName = displayName;
            EchoId = echoId;
            Kind = kind;
            Color = color;
            ActiveNote = activeNote;
            EchoNote = echoNote;
        }

        public string Id { get; }
        public string DisplayName { get; }
        public string EchoId { get; }
        public PrototypeEffectKind Kind { get; }
        public Color Color { get; }
        public string ActiveNote { get; }
        public string EchoNote { get; }
    }

    public sealed class PrototypeEchoSpec
    {
        public PrototypeEchoSpec(string id, string displayName, string sourceMemoryId, PrototypeEffectKind kind, Color color)
        {
            Id = id;
            DisplayName = displayName;
            SourceMemoryId = sourceMemoryId;
            Kind = kind;
            Color = color;
        }

        public string Id { get; }
        public string DisplayName { get; }
        public string SourceMemoryId { get; }
        public PrototypeEffectKind Kind { get; }
        public Color Color { get; }
    }

    public sealed class PrototypeSynergySpec
    {
        public PrototypeSynergySpec(string id, string displayName, string[] requiredEchoIds, Color color, string note)
        {
            Id = id;
            DisplayName = displayName;
            RequiredEchoIds = requiredEchoIds;
            Color = color;
            Note = note;
        }

        public string Id { get; }
        public string DisplayName { get; }
        public string[] RequiredEchoIds { get; }
        public int RequiredLevel => 5;
        public Color Color { get; }
        public string Note { get; }
    }
}
