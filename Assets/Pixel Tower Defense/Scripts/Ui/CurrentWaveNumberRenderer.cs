using Assets.Scripts.Waves;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Ui
{
    /// <summary>
    /// UI component that renders current wave number
    /// The code is self-explanatory
    /// </summary>
    [RequireComponent(typeof(Text))]
    public class CurrentWaveNumberRenderer : MonoBehaviour
    {
        private void Awake()
        {
            var waveSpawner = FindObjectOfType<WaveSpawner>();
            if (waveSpawner != null)
            {
                waveSpawner.CurrentWaveIndexHasChangedEvent += ChangeCurrentWaveNumberText;
            }
        }

        private void ChangeCurrentWaveNumberText(int waveNumber)
        {
            GetComponent<Text>().text = waveNumber.ToString();
        }
    }
}