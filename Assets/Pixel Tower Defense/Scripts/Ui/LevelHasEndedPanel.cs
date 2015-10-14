using Assets.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Ui
{
    /// <summary>
    /// Panel with "Level has ended" logic
    /// Contains different controls and text fields
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class LevelHasEndedPanel : MonoBehaviour
    {
        public GameObject GameOverTopText;
        public GameObject MissionAccomplishedTopText;
        public Text BasicScoreText;
        public Text LifeBonusText;
        public Text TotalScoreText;
        public GameObject NextButton;

        private Animator _animator;

        /// <summary>
        /// Panel rendering logic
        /// </summary>
        /// <param name="hasFailed">Whether we have failed, i.e. it's a "win" or a "game over"</param>
        public void DisplayLevelHasEndedPanel(bool hasFailed)
        {
            // Put this panel to the front of the screen
            transform.SetAsLastSibling();

            // Play fade in animation
            GetComponent<Animator>().SetBool("IsActive", true);

            // Maybe display "Next" button
            NextButton.SetActive(!hasFailed);

            // If it's a game over we display "Game over" text
            if (hasFailed)
            {
                MissionAccomplishedTopText.SetActive(false);
                GameOverTopText.SetActive(true);
            }
            // If not, we display "Mission accomplished" text
            else
            {
                GameOverTopText.SetActive(false);
                MissionAccomplishedTopText.SetActive(true);
            }

            // Filling our text fields

            var basicScore = PlayerStatus.Instance.Score;
            var lifeBonus = (int)((float)PlayerStatus.Instance.Lives / PlayerStatus.Instance.MaxLives * 2000f);

            BasicScoreText.text = basicScore.ToString();
            LifeBonusText.text = lifeBonus.ToString();
            TotalScoreText.text = (basicScore + lifeBonus).ToString();
        }
    }
}