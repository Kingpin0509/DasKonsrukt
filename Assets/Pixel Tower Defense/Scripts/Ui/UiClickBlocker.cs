using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.Ui
{
    /// <summary>
    /// Helper class for blocking clicks (when choosing the towers to buy for example)
    /// Also contains logic for handling user clicks (so we know when the user didn't choose anything)
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class UiClickBlocker : BaseGameObject, IPointerClickHandler
    {
        /// <summary>
        /// Simple singleton logic
        /// </summary>
        public static UiClickBlocker Instance { get; private set; }

        /// <summary>
        /// Instance event, fires when this click blocker is clicked
        /// </summary>
        public event Action BlockerWasClickedEvent;

        protected override void Awake()
        {
            base.Awake();
            Instance = this;
            GameObjectCache.SetActive(false);
        }

        /// <summary>
        /// Simple event invocation logic upon clicking on the click blocker
        /// </summary>
        /// <param name="eventData">Data that is sent with an event</param>
        public void OnPointerClick(PointerEventData eventData)
        {
            if (BlockerWasClickedEvent != null)
            {
                BlockerWasClickedEvent();
            }
        }

        public void Enable()
        {
            GameObjectCache.SetActive(true);
        }

        public void Disable()
        {
            GameObjectCache.SetActive(false);
        }
    }
}
