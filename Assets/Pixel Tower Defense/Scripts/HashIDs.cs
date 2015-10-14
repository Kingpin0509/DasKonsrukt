using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// Helper class made to cache the string representations of the Animator properties
    /// Check the Stealth project tutorial about HashIDs
    /// http://unity3d.com/learn/tutorials/projects/stealth/hashids
    /// </summary>
    public class HashIDs : MonoBehaviour
    {
        public static int Angle { get; private set; }
        public static int Fire { get; private set; }
        public static int Die { get; private set; }
        public static int Blow { get; private set; }

        private void Start()
        {
            Angle = Animator.StringToHash("Angle");
            Fire = Animator.StringToHash("Fire");
            Die = Animator.StringToHash("Die");
            Blow = Animator.StringToHash("Blow");
        }
    }
}
