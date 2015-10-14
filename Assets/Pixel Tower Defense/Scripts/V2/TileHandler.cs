using Assets.Scripts.Tile;
using UnityEngine;

namespace Assets.Scripts.V2
{
    /// <summary>
    /// Tile handler base class
    /// Contains overridable logic
    /// </summary>
    public abstract class TileHandler : MonoBehaviour
    {
        public abstract void ProcessTile(ChoiceItem choiceItem);
    }
}