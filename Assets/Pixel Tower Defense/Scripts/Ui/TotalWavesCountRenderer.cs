using Assets.Scripts.Waves;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Ui
{
    /// <summary>
    /// Simple class for total waves count renderer
    /// </summary>
    [RequireComponent(typeof(Text))]
    public class TotalWavesCountRenderer : MonoBehaviour
    {
        private void Awake()
        {
            // On awake, we find the wave spawner
            var waveSpawner = GameObject.FindObjectOfType<WaveSpawner>();

            // And if there is one
            if (waveSpawner != null)
            {
                // We change our text caption
                GetComponent<Text>().text = waveSpawner.TotalWavesCount.ToString();
            }
            else
            {
                Debug.LogError("Unable to find Wave Spawner");
            }
        }
    }
}