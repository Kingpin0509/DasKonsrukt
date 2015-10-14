using Assets.Scripts.Tile;
using Assets.Utils;
using UnityEngine;

namespace Assets.Scripts.V2
{
    /// <summary>
    /// Tower buyer handler
    /// Contains logic for tower buy process
    /// It's a "Tile handler" because it handles the tower pad tile
    /// </summary>
    public class TowerBuyer : TileHandler
    {
        public override void ProcessTile(ChoiceItem choiceItem)
        {
            // Getting the tile (to know it's transform)
            var tile = choiceItem.SceneItem;

            // And our chosen tower, which we will instantiate
            var towerPrefab = choiceItem.Tile;

            // If we buy, we have to spend some money
            PlayerStatus.Instance.Money -= choiceItem.Cost;

            Instantiate(towerPrefab, tile.TransformCache.position, Quaternion.identity);

            // We should also destroy the tower pad
            choiceItem.SceneItem.Destroy();
        }
    }
}