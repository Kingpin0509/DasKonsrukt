using UnityEngine;

namespace Assets.Scripts.Ui.MainMenuFsmBehaviours
{
    /// <summary>
    /// Main screen state mecanim behaviour
    /// </summary>
    public class MainScreenState : StateMachineBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var commonController = animator.GetComponent<CommonMenuController>();
            commonController.MainMenuAnimator.SetBool("IsActive", true);
            commonController.OptionsMenuAnimator.SetBool("IsActive", false);
            commonController.AboutMenuAnimator.SetBool("IsActive", false);
        }
    }
}