using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Editor
{
    /// <summary>
    /// Simple AssetPostprocessor that changes the filter mode of all new textures to Point
    /// </summary>
    public class PointFilterAssetPostprocessor : AssetPostprocessor
    {
        private void OnPostprocessTexture(Texture2D texture)
        {
            texture.filterMode = FilterMode.Point;
        }
    }
}
