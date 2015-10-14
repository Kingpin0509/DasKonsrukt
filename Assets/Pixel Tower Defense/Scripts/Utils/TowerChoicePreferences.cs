using Assets.Scripts.Tile;
using UnityEngine;

namespace Assets.Utils
{
    /// <summary>
    /// Helper class that holds the tower choices
    /// We choose which one to buy based on its ChosenChoiceItems
    /// </summary>
    public class TowerChoicePreferences : MonoBehaviour
    {
        /// <summary>
        /// Simple singleton logic
        /// </summary>
        public static TowerChoicePreferences Instance { get; private set; }

        /// <summary>
        /// Currently linked tower choice pack
        /// Visible in the editor
        /// </summary>
        public ChoicePack ChosenChoiceItems;
        
        void Awake()
        {
            // If there's no instance yet
            if (Instance == null)
            {
                // We say that this is the one
                Instance = this;

                // And don't let it die throughout the levels
                DontDestroyOnLoad(this);
            }
            else
            {
                // But if there's already one on the scene
                // we destroy it and let the older one live
                Destroy(this.gameObject);
            }
        }
    }
}
