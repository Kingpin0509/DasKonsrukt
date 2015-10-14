using Assets.Scripts.Tile;
using Assets.Scripts.Towers;
using Assets.Utils;
using UnityEngine;

namespace Assets.Scripts.V2
{
    /// <summary>
    /// Tower upgrader logic
    /// Upgrades the tower and decreases player's money
    /// </summary>
    public class TowerUpgrader : TileHandler
    {
        public override void ProcessTile(ChoiceItem choiceItem)
        {
            PlayerStatus.Instance.Money -= choiceItem.Cost;
            var tower = choiceItem.SceneItem.GetComponent<Tower>();
            tower.Rank += 1;
        }
    }
}