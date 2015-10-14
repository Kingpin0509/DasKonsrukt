using System;
using Assets.EventSystem;
using Assets.EventSystem.Events;
using Assets.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Ui
{
    /// <summary>
    /// Pause button class that contains sprite swapping logic
    /// It's different whether we are in "Paused" mode or in "Play" mode
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class PauseButton : MonoBehaviour
    {
        public Sprite PauseSprite;
        public Sprite PlaySprite;

        private void Awake()
        {
            // We don't have to unsubscribe because it will be cleaned up by the GlobalEventSystem
            GlobalEventSystem<GamePausedEvent>.EventHappened += OnGamePaused;
        }

        private void OnGamePaused(GamePausedEvent gamePausedEvent)
        {
            ChangePauseSprite(gamePausedEvent.IsPaused);
        }

        public void PauseGame()
        {
            bool isPaused = !GameStatus.Instance.IsPaused;

            ChangePauseSprite(isPaused);

            GameStatus.Instance.PauseGame(isPaused);
        }

        private void ChangePauseSprite(bool isPaused)
        {
            // Simple ternary operator. We change our button sprite to be appropriate
            GetComponent<Image>().sprite = isPaused ? PlaySprite : PauseSprite;
        }
    }
}