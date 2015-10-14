using System.IO;
using System.Text.RegularExpressions;
using Assets.Scripts.Waves;
using Assets.Utils;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Editor
{
    public class SetupScene : EditorWindow
    {
        [MenuItem("GameObject/Pixel Tower Defense/Setup level")]
        public static void SetupLevel()
        {
            GetWindow<SetupScene>("Level setup");
        }

        public enum LevelStyle
        {
            Badlands,
            Desert,
            Grass,
            Snow
        }

        private LevelStyle _currentLevelStyle;

        public void OnGUI()
        {
            // First, we create a GUI combo box element
            _currentLevelStyle = (LevelStyle)EditorGUILayout.EnumPopup("Level style", _currentLevelStyle);

            // Then, upon the button press we add all the assets in the "Auto setup" folder on our scene
            if (GUILayout.Button("Setup"))
            {
                // First, get an absolute path of our current project and an Auto setup folder in it
                var setupFilesRootPath = Application.dataPath + "/Prefabs/Auto setup";

                // For each file found in this folder
                foreach (var file in Directory.GetFiles(setupFilesRootPath))
                {
                    // We only get .prefab files, skipping the .meta ones
                    if (file.EndsWith(".prefab"))
                    {
                        // Simple regular expression to get relative path to the asset
                        var relativePathMatch = Regex.Match(file, @"(Assets/.+)");
                        // If the regular expression matching was successfull
                        if (relativePathMatch.Success)
                        {
                            // We extract matched value (it will be our asset path)
                            var relativePath = relativePathMatch.Groups[0].Value;
                            // Then we load that asset to the main memory, so we can Instantiate it later
                            var asset = AssetDatabase.LoadAssetAtPath(relativePath, typeof(GameObject));

                            // Helping object so we can replace the "(Clone)" in its name later
                            Object newAsset;

                            // Trying to find an object on the scene
                            var presentGameObject = GameObject.Find(asset.name);
                            // If there's one
                            if (presentGameObject != null)
                            {
                                // We destroy it first
                                DestroyImmediate(presentGameObject);
                            }
                            // And we create 
                            newAsset = PrefabUtility.InstantiatePrefab(asset);

                            // Then we replace that nasty "(Clone)" string in it's name so we can find it later 
                            newAsset.name = newAsset.name.Replace("(Clone)", "");
                        }
                    }
                }

                // Filling the level with Grid tile of selected style
                var gridPath = "Assets/Prefabs/Levels/" + _currentLevelStyle.ToString() + "/Grid.prefab";
                var gridAsset = AssetDatabase.LoadAssetAtPath(gridPath, typeof(GameObject));
                var halfLevelWidth = FindObjectOfType<AdjustCameraHeight>().ActiveZoneWidth / 2f;
                var halfLevelHeght = FindObjectOfType<Camera>().orthographicSize;

                var gridRoot = DestroyAndCreate("Grid root");

                // Generation our actual BG grid of the chosen theme
                for (int i = Mathf.FloorToInt(-halfLevelWidth); i <= halfLevelWidth + 1; i++)
                {
                    for (int j = Mathf.FloorToInt(-halfLevelHeght); j <= Mathf.CeilToInt(halfLevelHeght); j++)
                    {
                        var grid = Instantiate(gridAsset) as GameObject;
                        grid.transform.position = new Vector3(i, j);
                        grid.transform.SetParent(gridRoot.transform);
                    }
                }
            }
        }

        private GameObject DestroyAndCreate(string name)
        {
            var gameObject = GameObject.Find(name);
            if (gameObject != null)
            {
                DestroyImmediate(gameObject);
            }

            gameObject = new GameObject(name);
            gameObject.name = name;
            return gameObject;
        }
    }
}