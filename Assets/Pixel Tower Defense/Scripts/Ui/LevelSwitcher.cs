using System.Collections;
using Assets.EventSystem;
using Assets.EventSystem.Events;
using UnityEngine;

namespace Assets.Scripts.Ui
{
    public class LevelSwitcher : MonoBehaviour
    {
        public GameObject LoadingScreen;
        private CanvasGroup _loadingScreenCanvasGroup;

        private void Awake()
        {
            _loadingScreenCanvasGroup = LoadingScreen.GetComponent<CanvasGroup>();
            if (LoadingScreen.activeSelf)
            {
                LoadingScreen.SetActive(false);
            }
        }

        public void LoadLevel(string levelName)
        {
            if (levelName == "SAME"|| Application.CanStreamedLevelBeLoaded(levelName))
            {
                StartCoroutine(FadeInCoroutine(levelName));
            }
            else
            {
                Debug.LogError("Scene " + levelName + " does not exist");
            }
        }

        IEnumerator FadeInCoroutine(string levelName)
        {
            // It's constant here for simplicity
            const float fadeInSpeed = 3f;

            // We also set our loading screen alpha to 0 so we can fade it in from 0
            _loadingScreenCanvasGroup.alpha = 0f;

            // Preparing the screen (see the method for info)
            PrepareLoadingScreen();

            while (_loadingScreenCanvasGroup.alpha < 0.99f)
            {
                // Right here we have timescale of 0
                // so we need to animate with the unscaledDeltaTime
                _loadingScreenCanvasGroup.alpha += Time.unscaledDeltaTime * fadeInSpeed;
                yield return null;
            }
            
            // First, we make it clear to anyone that we are loading a new level
            GlobalEventSystem<LoadingNewLevelEvent>.Raise(new LoadingNewLevelEvent());

            // By this moment the alpha is about 0.9, so we hard set it to 1
            _loadingScreenCanvasGroup.alpha = 1f;

            // Set our timescale to 1
            Time.timeScale = 1f;

            // And clean up the global event system
            // By this time every subscriber has already processed the LoadingNewLevelEvent
            GlobalEventSystemMaitenance.CleanupEventSystem();

            // We load the same level if we are to do so
            if (levelName != "SAME")
            {
                LoadLevelWithName(levelName);
            }
            // Or we load a specified level if it's not the "SAME"
            else
            {
                LoadLevelWithName(Application.loadedLevelName);
            }
        }

        /// <summary>
        /// Helper method to prepare the loading screen
        /// It activates the Loading screen, sets it as a child of the canvas (if it is already - does nothing)
        /// and then sets it as the top "layer" of the screen
        /// </summary>
        private void PrepareLoadingScreen()
        {
            LoadingScreen.SetActive(true);
            LoadingScreen.transform.SetParent(transform.parent);
            LoadingScreen.transform.SetAsLastSibling();
        }

        /// <summary>
        /// Helper method, additional logic is reserved for future possible changes
        /// </summary>
        /// <param name="levelName">Name of the level to load</param>
        private void LoadLevelWithName(string levelName)
        {
            Application.LoadLevel(levelName);
        }
    }
}