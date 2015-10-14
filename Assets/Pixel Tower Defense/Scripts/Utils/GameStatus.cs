using System;
using Assets.EventSystem;
using Assets.EventSystem.Events;
using UnityEngine;

namespace Assets.Utils
{
    /// <summary>
    /// Helper class that allows simple pausing logic (along with checks of current pause state)
    /// Can be extended freely
    /// </summary>
    public class GameStatus
    {
        /// <summary>
        /// Singleton instance field
        /// </summary>
        private static GameStatus _instance;

        /// <summary>
        /// Singleton instance property
        /// </summary>
        public static GameStatus Instance
        {
            // If our instance field is null, we create one
            get { return _instance ?? (_instance = new GameStatus()); }
        }

        /// <summary>
        /// Property to check of the game is currently paused
        /// </summary>
        public bool IsPaused { get; private set; }

        /// <summary>
        /// Custom class constructor
        /// Some additional logic is needed upon object creation
        /// </summary>
        private GameStatus()
        {
            // We need to subscribe to the level loading event so we know when we should drop the GameStatus state
            GlobalEventSystem<LoadingNewLevelEvent>.EventHappened += GameHasEndedHandler;
        }

        /// <summary>
        /// Level loading event handler
        /// </summary>
        /// <param name="gameHasEndedEvent">Empty event data</param>
        private void GameHasEndedHandler(LoadingNewLevelEvent gameHasEndedEvent)
        {
            // When we hit this event, it means that we should clean up the object
            // by unsubscribing from the LoadingNewLevelEvent
            GlobalEventSystem<LoadingNewLevelEvent>.EventHappened -= GameHasEndedHandler;

            // We also set our static instance to null so we will create a new object once someone request an Instance
            // the current object, however, will be abandoned and will eventually be collected by the GC (Garbage Collector)
            _instance = null;
        }

        /// <summary>
        /// Method to pause the game
        /// </summary>
        /// <param name="isPaused"></param>
        public void PauseGame(bool isPaused)
        {
            IsPaused = isPaused;
            Time.timeScale = IsPaused ? 0f : 1f;
            GlobalEventSystem<GamePausedEvent>.Raise(new GamePausedEvent(IsPaused));
        }
    }
}