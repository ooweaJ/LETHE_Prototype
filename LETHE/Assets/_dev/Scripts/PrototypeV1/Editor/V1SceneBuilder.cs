using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Lethe.PrototypeV1.Editor
{
    public static class V1SceneBuilder
    {
        const string ScenePath = "Assets/_dev/Scenes/Dev_Prototype_v1.unity";

        [MenuItem("LETHE/_dev/Rebuild Prototype v1 Scene")]
        public static void Build()
        {
            EnsureFolders();
            ConfigureSprites();

            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            scene.name = "Dev_Prototype_v1";

            var cameraObject = new GameObject("Main Camera");
            cameraObject.tag = "MainCamera";
            cameraObject.transform.position = new Vector3(0f, 0f, -10f);
            var camera = cameraObject.AddComponent<Camera>();
            camera.orthographic = true;
            camera.orthographicSize = 6.1f;
            camera.backgroundColor = new Color(0.035f, 0.045f, 0.055f);
            camera.clearFlags = CameraClearFlags.SolidColor;

            var lightObject = new GameObject("V1_Readability_Light");
            var light = lightObject.AddComponent<Light>();
            light.type = LightType.Directional;
            light.intensity = 0.35f;
            lightObject.transform.rotation = Quaternion.Euler(50f, -30f, 0f);

            var manager = new GameObject("V1_GameManager");
            var gameManager = manager.AddComponent<V1GameManager>();
            WireManagerReferences(gameManager);

            EditorSceneManager.SaveScene(scene, ScenePath);
            Selection.activeGameObject = manager;
            Debug.Log($"Built fresh LETHE prototype scene: {ScenePath}");
        }

        static void WireManagerReferences(V1GameManager gameManager)
        {
            var serialized = new SerializedObject(gameManager);
            serialized.FindProperty("contentCatalog").objectReferenceValue =
                AssetDatabase.LoadAssetAtPath<V1ContentCatalog>("Assets/_dev/Data/V1_ContentCatalog.asset");
            serialized.FindProperty("dualBladesDefinition").objectReferenceValue =
                AssetDatabase.LoadAssetAtPath<Lethe.Dev.WeaponDefinition>("Assets/_dev/Data/Weapons/Weapon_DualBlades.asset");
            serialized.FindProperty("greatswordDefinition").objectReferenceValue =
                AssetDatabase.LoadAssetAtPath<Lethe.Dev.WeaponDefinition>("Assets/_dev/Data/Weapons/Weapon_Greatsword.asset");
            serialized.ApplyModifiedPropertiesWithoutUndo();
        }

        static void EnsureFolders()
        {
            CreateFolder("Assets/_dev/Scripts", "PrototypeV1");
            CreateFolder("Assets/_dev/Scripts/PrototypeV1", "Editor");
            CreateFolder("Assets/_dev/Scenes", string.Empty);
            CreateFolder("Assets/_dev/Data", string.Empty);
        }

        static void CreateFolder(string parent, string child)
        {
            if (string.IsNullOrEmpty(child))
            {
                if (!Directory.Exists(parent)) Directory.CreateDirectory(parent);
                return;
            }

            var path = $"{parent}/{child}";
            if (!AssetDatabase.IsValidFolder(path))
            {
                AssetDatabase.CreateFolder(parent, child);
            }
        }

        static void ConfigureSprites()
        {
            foreach (var path in AssetDatabase.FindAssets("t:Texture2D", new[] { "Assets/_dev/Art/Sprites" }))
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(path);
                var importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
                if (importer == null) continue;

                var changed = false;
                if (importer.textureType != TextureImporterType.Sprite)
                {
                    importer.textureType = TextureImporterType.Sprite;
                    changed = true;
                }

                if (importer.spritePixelsPerUnit != 100f)
                {
                    importer.spritePixelsPerUnit = 100f;
                    changed = true;
                }

                if (importer.filterMode != FilterMode.Point)
                {
                    importer.filterMode = FilterMode.Point;
                    changed = true;
                }

                if (changed) importer.SaveAndReimport();
            }
        }
    }
}
