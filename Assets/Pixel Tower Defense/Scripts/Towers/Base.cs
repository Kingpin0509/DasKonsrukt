using System;
using Assets.EventSystem;
using Assets.EventSystem.Events;
using Assets.Utils;
using UnityEngine;

namespace Assets.Scripts.Towers
{
    /// <summary>
    /// Class of the Base tower. It's the tower that we are to protect
    /// </summary>
    [RequireComponent(typeof(CircleCollider2D))]
    public class Base : MonoBehaviour
    {

        // Supressing "Never assigned" warning
#pragma warning disable 0649

        /// <summary>
        /// Sprites of the Base tower
        /// Different states of "health"
        /// It's hardcoded to 4 states, so one should do something with this code to allow more different states
        /// </summary>
        [SerializeField]
        private Sprite[] _sprites;

#pragma warning restore 0649

        /// <summary>
        /// Cached sprite renderer component
        /// </summary>
        private SpriteRenderer _renderer;

        /// <summary>
        /// Simple setup code
        /// </summary>
        private void Start()
        {
            // We subscribe to LiveChanged event
            // It's enemies that check if they collide with the base tower
            // they also call the HandleEnemyHit function here
            // it's just a matter of taste - who is to handle this Lives logic 
            GlobalEventSystem<LiveCountHasChangedEvent>.EventHappened += LivesChangedEventHandler;
            _renderer = GetComponent<SpriteRenderer>();
        }

        /// <summary>
        /// Handler is called when lives have changed
        /// We change the tower sprite here based on current live ratio
        /// </summary>
        /// <param name="liveCountHasChangedEvent">Event data</param>
        private void LivesChangedEventHandler(LiveCountHasChangedEvent liveCountHasChangedEvent)
        {
            // Getting our lives ratio, value from 0 to 1
            var ratio = LivesRatio();

            // If we're pretty healthy, set the sprite to the second one
            if (ratio < 0.75f && ratio > 0.4f)
            {
                _renderer.sprite = _sprites[1];
            }
            // If we're not that healthy, set the sprite to the third one
            else if (ratio < 0.4f && ratio > 0.1f)
            {
                _renderer.sprite = _sprites[2];
            }
            // If we're dead, set to the dead sprite
            else if (ratio < 0.1f)
            {
                _renderer.sprite = _sprites[3];
            }
        }

        /// <summary>
        /// Helper function to calculate the ratio
        /// </summary>
        /// <returns>Current lives ratio, number from 0 to 1</returns>
        private float LivesRatio()
        {
            return (float)PlayerStatus.Instance.Lives / PlayerStatus.Instance.MaxLives;
        }

        /// <summary>
        /// A method that handles enemy hits
        /// Live count is decreased here
        /// It's done so that there's just one place where we can place some additional logic
        /// for example - invisibility for a short period of time
        /// </summary>
        public void HandleEnemyHit()
        {
            PlayerStatus.Instance.Lives--;
        }
    }
}
