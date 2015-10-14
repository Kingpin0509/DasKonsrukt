using Assets.Scripts.Tile;
using Assets.Scripts.V2;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Editor
{
    /// <summary>
    /// A class that allows to create instances of scriptable objects
    /// </summary>
    public class CreateCustomAssets : EditorWindow
    {
        [MenuItem("Assets/Create/Pixel Tower Defense/Choice Pack")]
        public static void CreateChoicePackMenu()
        {
            var choicePack = ScriptableObject.CreateInstance<ChoicePack>();
            var currentPath = AssetDatabase.GetAssetPath(Selection.activeObject);

            AssetDatabase.SaveAssets();
            ProjectWindowUtil.CreateAsset(choicePack,
                AssetDatabase.GenerateUniqueAssetPath(currentPath + "/Choice pack") + ".asset");
        }
    }
}