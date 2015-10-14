using UnityEngine;

namespace Assets.Scripts.Ui.MainMenuFsmBehaviours
{
    /// <summary>
    /// Utility class that handles transitions from different screens
    /// In the main menu we have three of them
    /// It also contains logic for buttons ( *WasPressed() methods)
    /// </summary>
    public class CommonMenuController : MonoBehaviour
    {
        private const int MainScreenIndex = 0;
        private const int OptionsScreenIndex = 1;
        private const int AboutScreenIndex = 2;

        /// <summary>
        /// Behaviour animator
        /// Doesn't have any "Animation" logic, only consists of StateMachineBehaviour states
        /// </summary>
        private Animator _animator;

        // -------
        // Common access point for all screen animators (they are used by different screens themselves
        public Animator MainMenuAnimator { get; private set; }
        public Animator OptionsMenuAnimator { get; private set; }
        public Animator AboutMenuAnimator { get; private set; }
        // -------

        private void Awake()
        {
            _animator = GetComponent<Animator>();

            var transformLocalCache = GetComponent<Transform>();
            MainMenuAnimator = transformLocalCache.FindChild("Start Screen").GetComponent<Animator>();
            OptionsMenuAnimator = transformLocalCache.FindChild("Options Screen").GetComponent<Animator>();
            AboutMenuAnimator = transformLocalCache.FindChild("About Screen").GetComponent<Animator>();
        }

        public void StartButtonWasPressed()
        {
            transform.parent.GetComponentInChildren<LevelSwitcher>().LoadLevel("Level Selection Scene");
        }

        public void OptionsButtonWasPressed()
        {
            _animator.SetInteger("Screen Index", OptionsScreenIndex);
        }

        public void BackButtonWasPressed()
        {
            _animator.SetInteger("Screen Index", MainScreenIndex);
        }

        public void AboutButtonWasPressed()
        {
            _animator.SetInteger("Screen Index", AboutScreenIndex);
        }
    }
}