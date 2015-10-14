using UnityEngine;

namespace Assets.Scripts.Utils
{
    /// <summary>
    /// Helper class that stays in the scene and provides a link to the prefab with the valid tower point of needed color
    /// This allows us to arbitrarily change the tower point color from level to level
    /// For example, Badlands have light themed tower pads while other levels have dark ones
    /// </summary>
    public class ValidTowerPointProvider : MonoBehaviour
    {
        /// <summary>
        /// Editor linked tower pad prefab
        /// </summary>
        public GameObject ValidTowerPoint;

        /// <summary>
        /// Simple singleton logic
        /// </summary>
        public static ValidTowerPointProvider Instance { get; private set; }

        private void Awake()
        {
            // Simply override everything
            Instance = this;
        }
    }
}