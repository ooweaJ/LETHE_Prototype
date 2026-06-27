using System;
using Lethe.Dev;
using UnityEngine;

namespace Lethe.PrototypeV1
{
    [Serializable]
    public sealed class V1SpriteCatalogEntry
    {
        public string id;
        public Sprite sprite;
    }

    [CreateAssetMenu(menuName = "LETHE/Prototype V1/Content Catalog")]
    public sealed class V1ContentCatalog : ScriptableObject
    {
        public WeaponDefinition dualBladesDefinition;
        public WeaponDefinition greatswordDefinition;
        public Font koreanFont;
        public V1SpriteCatalogEntry[] sprites = Array.Empty<V1SpriteCatalogEntry>();

        public Sprite SpriteForPath(string path)
        {
            if (string.IsNullOrEmpty(path) || sprites == null) return null;

            for (var index = 0; index < sprites.Length; index += 1)
            {
                var entry = sprites[index];
                if (entry == null || entry.sprite == null) continue;
                if (string.Equals(entry.id, path, StringComparison.OrdinalIgnoreCase))
                {
                    return entry.sprite;
                }
            }

            return null;
        }

        public Texture2D TextureForPath(string path)
        {
            var sprite = SpriteForPath(path);
            return sprite != null ? sprite.texture : null;
        }
    }
}
