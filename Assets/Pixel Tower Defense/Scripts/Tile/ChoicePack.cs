using UnityEngine;

namespace Assets.Scripts.Tile
{
    /// <summary>
    /// Simple storage for a custom scriptable object
    /// It holds all the ChoiceItems you throw to it
    /// </summary>
    public class ChoicePack : ScriptableObject
    {
        /// <summary>
        /// Actual choice items array
        /// [SerializeField] attribute for explicitness
        /// </summary>
        [SerializeField]
        public ChoiceItem[] ChoiceItems;
    }
}