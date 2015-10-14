using Assets.EventSystem;
using Assets.EventSystem.Events;
using Assets.Scripts.Towers;
using Assets.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Tile
{
    /// <summary>
    /// Tower upgrade choice item
    /// A choice item that appears when a user clicks on an active tower
    /// Strange inheritanse here is done for simplicity
    /// Both classes share almost the same behavior
    /// We could as well inherit SellChoice from UpgraderChoice
    /// </summary>
    public class TowerUpgradeChoiceItem : TowerSellChoiceItem
    {
        /// <summary>
        /// Changing the cost logic
        /// We're not only getting the cost, but also dimming the Upgrade button
        /// </summary>
        /// <param name="tower"></param>
        /// <returns></returns>
        protected override int GetCostFromTower(Tower tower)
        {
            var towerUpgradePrice = tower.TowerPreferences[tower.Rank].UpgradeCost;

            if (towerUpgradePrice > PlayerStatus.Instance.Money)
            {
                GetComponent<Image>().color = Color.gray;
                // And subscribe to the money changed event so possobly later we will be able to buy that tower
                GlobalEventSystem<MoneyChangedEvent>.EventHappened += MoneyHasChangedEvent;
            }

            return towerUpgradePrice;
        }

        /// <summary>
        /// Money changed event handler
        /// We check whether we NOW have enough money to upgrade the tower
        /// </summary>
        /// <param name="moneyChangedEvent">The event data</param>
        private void MoneyHasChangedEvent(MoneyChangedEvent moneyChangedEvent)
        {
            if (PlayerStatus.Instance.Money >= Cost)
            {
                GetComponent<Image>().color = Color.white;
                // We unsubscribe because for now there's no way the money will decay
                GlobalEventSystem<MoneyChangedEvent>.EventHappened -= MoneyHasChangedEvent;
            }
        }

        /// <summary>
        /// Don't forget to unsubscribe upon deletion
        /// </summary>
        private void OnDestroy()
        {
            GlobalEventSystem<MoneyChangedEvent>.EventHappened -= MoneyHasChangedEvent;
        }
    }
}