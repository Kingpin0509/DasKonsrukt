using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// Path tile class, represents the tile on which the enemies walk
    /// </summary>
    public class PathTile : MonoBehaviour
    {
        /// <summary>
        /// Next linked tile backing field (so we can serialize it, Unity isn't good at serializing properties)
        /// </summary>
        [SerializeField]
        private PathTile _nextTile;

        /// <summary>
        /// Property for consistency
        /// </summary>
        public PathTile NextTile
        {
            get { return _nextTile; }
            set { _nextTile = value; }
        }
    }
}
