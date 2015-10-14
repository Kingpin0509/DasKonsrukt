using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.V2
{
    /// <summary>
    /// Helper class of tower prefs with custom indexer
    /// </summary>
    public class TowerPrefs : MonoBehaviour
    {
        public List<RankPrefs> RankPrefs;

        /// <summary>
        /// Custom indexer that allows accessing rank prefs directly by index
        /// </summary>
        /// <param name="rank">Needed index from which to get the prefs</param>
        /// <returns></returns>
        public RankPrefs this[int rank]
        {
            get
            {
                // Simple IndexOutOfRangeException safeguard
                int count = RankPrefs.Count;
                int rankIndex = rank > count ? count : rank;

                return RankPrefs[rankIndex - 1];
            }
        }
    }
}