using System;
using Assets.EventSystem;
using Assets.EventSystem.Events;
using Assets.Scripts.Ui;
using UnityEngine;

namespace Assets.Utils
{
    /// <summary>
    /// Helper class that handles "Game over" process when lives count goes to 0
    /// Can be merged with some other class in the future, but right now its logic is pretty separated
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        /// <summary>
        /// Editor linked panel that is shown upon "game over"
        /// </summary>
        public LevelHasEndedPanel LevelHasEndedPanel;

        private void Awake()
        {
            // Simple assertion
            if (LevelHasEndedPanel == null)
            {
                Debug.LogError("The \"Level Has Ended Panel\" is not linked to the Game Manager");
            }

            GlobalEventSystem<LiveCountHasChangedEvent>.EventHappened += LiveCountHasChangedEventHandler;
            GlobalEventSystem<GameHasEndedEvent>.EventHappened += GameHasEndedEventHandler;
        }

        private void LiveCountHasChangedEventHandler(LiveCountHasChangedEvent liveCountHasChangedEvent)
        {
            // We do the logic only if lives count reached zero
            if (liveCountHasChangedEvent.Lives == 0)
            {
                GlobalEventSystem<GameHasEndedEvent>.EventHappened -= GameHasEndedEventHandler;
                GlobalEventSystem<GameHasEndedEvent>.Raise(new GameHasEndedEvent(true));
                Time.timeScale = 0f;
                LevelHasEndedPanel.gameObject.SetActive(true);
                LevelHasEndedPanel.DisplayLevelHasEndedPanel(true);
            }
        }

        /// <summary>
        /// Cleanup method
        /// Unsubscribes and cleans everything
        /// </summary>
        /// <param name="gameHasEndedEvent">Data that is sent with an event</param>
        private void GameHasEndedEventHandler(GameHasEndedEvent gameHasEndedEvent)
        {
            GlobalEventSystem<GameHasEndedEvent>.EventHappened -= GameHasEndedEventHandler;
            LevelHasEndedPanel.gameObject.SetActive(true);
            LevelHasEndedPanel.DisplayLevelHasEndedPanel(false);
            GlobalEventSystemMaitenance.CleanupEventSystem();
        }
    }
}