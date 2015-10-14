using System;
using UnityEngine;

namespace Assets.Scripts.V2
{
    /// <summary>
    /// Helper data holding class for rank prefs
    /// Can be converted to scriptable object
    /// </summary>
    public class RankPrefs : MonoBehaviour
    {
        public int SellCost;
        public int UpgradeCost;
        public float Range;
        public float FireRate;
        public int Damage;
    }
}