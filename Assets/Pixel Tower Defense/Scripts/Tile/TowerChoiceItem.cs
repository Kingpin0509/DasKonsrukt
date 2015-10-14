using System;
using Assets.EventSystem;
using Assets.EventSystem.Events;
using Assets.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Tile
{
    /// <summary>
    /// A common choice item for towers
    /// It gets displayed when a user presses the valid tower point
    /// </summary>
    public class TowerChoiceItem : ChoiceItem
    {
        /// <summary>
        /// Manually linked Tower Graphics so we can dim it later if we don't have enough money
        /// </summary>
        public Image TowerGraphics;

        /// <summary>
        /// Manually linked Cost Text so we can easily change it's .text property
        /// </summary>
        public Text CostText;

        protected override void Awake()
        {
            base.Awake();
            Init();
        }

        /// <summary>
        /// Initialization code
        /// We dim the tower if we don't have enough money
        /// And set the cost text to the needed value
        /// 
        /// If we don't have enough money we also subscribe to the MoneyHasChangedEvent so we can clear the dim later
        /// </summary>
        public void Init()
        {
            // If the player doesn't have enough money
            if (PlayerStatus.Instance.Money < Cost)
            {
                // We dim the tower graphics
                TowerGraphics.color = Color.gray;

                // And subscribe to the money changed event so possobly later we will be able to buy that tower
                GlobalEventSystem<MoneyChangedEvent>.EventHappened += MoneyHasChangedEvent;
            }
            CostText.text = "$ " + Cost;
        }

        /// <summary>
        /// Money changed event handler
        /// We check whether we NOW have enough money to buy the tower
        /// </summary>
        /// <param name="moneyChangedEvent">The event data</param>
        private void MoneyHasChangedEvent(MoneyChangedEvent moneyChangedEvent)
        {
            if (PlayerStatus.Instance.Money >= Cost)
            {
                TowerGraphics.color = Color.white;
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