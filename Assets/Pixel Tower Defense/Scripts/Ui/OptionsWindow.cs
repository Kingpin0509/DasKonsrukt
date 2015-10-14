using Assets.Utils;
using UnityEngine;

namespace Assets.Scripts.Ui
{
    /// <summary>
    /// Options window logic
    /// Contains behaviour for the options window
    /// Code is mostly self-explanatory
    /// Animator stuff simply plays the fade in or fade out animation
    /// </summary>
    public class OptionsWindow : MonoBehaviour
    {
        public void OpenWindow()
        {
            gameObject.SetActive(true);
            transform.SetAsLastSibling();
            GameStatus.Instance.PauseGame(true);
            GetComponent<Animator>().SetBool("IsActive", true);               
        }

        public void CloseWindow()
        {
            GetComponent<Animator>().SetBool("IsActive", false);
            GameStatus.Instance.PauseGame(false);
            gameObject.SetActive(false);
        }
    }
}