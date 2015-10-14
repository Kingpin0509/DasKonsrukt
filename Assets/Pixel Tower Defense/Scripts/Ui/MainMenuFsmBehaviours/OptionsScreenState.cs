using UnityEngine;

namespace Assets.Scripts.Ui.MainMenuFsmBehaviours
{
    /// <summary>
    /// About screen state mecanim behaviour
    /// </summary>
    public class OptionsScreenState : StateMachineBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var commonController = animator.GetComponent<CommonMenuController>();
            commonController.MainMenuAnimator.SetBool("IsActive", false);
            commonController.OptionsMenuAnimator.SetBool("IsActive", true);
            commonController.AboutMenuAnimator.SetBool("IsActive", false);
        }
    }
}