using Assets.Scripts;
using UnityEngine;

namespace Assets.Utils
{
    /// <summary>
    /// Helper class that handles initial level setup on start
    /// There's a prefs prefab on the scene on which all the attributes are placed (for simplicity)
    /// </summary>
    public class SetupLevelOnStart : MonoBehaviour
    {
        public int StartingMoney;
        public int StartingLives = 1;

        private void Start()
        {
            PlayerStatus.Instance.Money = StartingMoney;
            PlayerStatus.Instance.Lives = StartingLives;
            PlayerStatus.Instance.MaxLives = StartingLives;
            ChoiceMenuLauncher.Initialize();
        }
    }
}