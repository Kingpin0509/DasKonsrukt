using Assets.Scripts.Towers;
using Assets.Scripts.V2;
using UnityEngine;

namespace Assets.Scripts.Tile
{
    /// <summary>
    /// Special component used to swap the choice pack of the tower when one upgrades it
    /// </summary>
    [RequireComponent(typeof(Tower))]
    [RequireComponent(typeof(LinkableSceneItem))]
    public class TowerChoicePackSwapper : MonoBehaviour
    {
        /// <summary>
        /// Target choice pack
        /// </summary>
        public ChoicePack ChoicePackToSwapTo;

        /// <summary>
        /// Linked scene item
        /// </summary>
        private LinkableSceneItem _linkableSceneItem;

        /// <summary>
        /// The tower of which we will swap the choice items
        /// </summary>
        private Tower _tower;

        private void Awake()
        {
            _tower = GetComponent<Tower>();
            _linkableSceneItem = GetComponent<LinkableSceneItem>();

            // We subscribe to the Tower's rank changed event so we know when we should swap the choice pack
            _tower.RankedHasChangedEvent += TowerOnRankedHasChangedEvent;
        }

        /// <summary>
        /// Handler of rank changed event
        /// Used to swap the choice pack of "Sell / Upgrade" pair to "Sell" only (upon reaching the last rank)
        /// </summary>
        /// <param name="i">New rank of the tower</param>
        private void TowerOnRankedHasChangedEvent(int i)
        {
            // Special case - if the upgrade cost is -1, we know that we've hit the last rank of the tower
            // and need to change our choice pack to Sell only
            if (_tower.TowerPreferences[i].UpgradeCost == -1)
            {
                _linkableSceneItem.ChoicePack = ChoicePackToSwapTo;
            }
        }
    }
}