using Assets.Scripts.Towers;
using Assets.Scripts.Utils;
using Assets.Scripts.V2;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Tile
{
    /// <summary>
    /// Tower sell choice item
    /// Handles the tower selling
    /// </summary>
    public class TowerSellChoiceItem : ChoiceItem
    {
        /// <summary>
        /// Tower component of the linked scene item
        /// </summary>
        private Tower _towerComponent;

        /// <summary>
        /// Link to the Text game object that represents cost of the sell
        /// </summary>
        public Text CostText;

        /// <summary>
        /// Overriden property behaviour
        /// Upon set, we get the item's tower component, 
        /// check it's sell cost and set our Text component to be valid
        /// </summary>
        public override LinkableSceneItem SceneItem
        {
            get { return base.SceneItem; }
            set
            {
                // Base logic, we already have a SceneItem in the base class
                base.SceneItem = value;
                _towerComponent = SceneItem.GetComponent<Tower>();
                base.Cost = GetCostFromTower(_towerComponent);
                CostText.text = Mathf.Abs(base.Cost).ToString();
            }
        }

        /// <summary>
        /// Overriden get tile logic
        /// It's needed for convinience, when we have levels with Light or Dark valid tower points
        /// </summary>
        /// <returns>Current valid tower point game object (light or dark)</returns>
        protected override GameObject GetTileLogic()
        {
            return ValidTowerPointProvider.Instance.ValidTowerPoint;
        }

        /// <summary>
        /// Gets cost from the provided tower
        /// </summary>
        /// <param name="tower">Tower from which we should get cost</param>
        /// <returns>Cost</returns>
        protected virtual int GetCostFromTower(Tower tower)
        {
            return -tower.TowerPreferences[tower.Rank].SellCost;
        }
    }
}