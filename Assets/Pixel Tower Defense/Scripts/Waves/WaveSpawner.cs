using System;
using System.Linq;
using Assets.EventSystem;
using Assets.EventSystem.Events;
using Assets.Utils;
using UnityEngine;

namespace Assets.Scripts.Waves
{
    /// <summary>
    /// Wave spawner class
    /// Contains logic for spawning waves
    /// The waves themselves are obtained via the editor linking
    /// </summary>
    public class WaveSpawner : MonoBehaviour
    {
        /// <summary>
        /// Editor visible Waves field
        /// Contains all the waves that need to be spawned
        /// </summary>
        public Wave[] Waves;

        /// <summary>
        /// Helper property
        /// A shortcut to array length
        /// </summary>
        public int TotalWavesCount { get { return Waves.Length; } }

        /// <summary>
        /// An event of changing the current wave index
        /// </summary>
        public event Action<int> CurrentWaveIndexHasChangedEvent;

        /// <summary>
        /// Simple property with a custom setter
        /// </summary>
        public int CurrentWaveIndex
        {
            get { return _currentWaveIndex; }
            set
            {
                if (_currentWaveIndex != value)
                {
                    _currentWaveIndex = value;

                    // If the value is different AND we have some subscribers - raise our event
                    if (CurrentWaveIndexHasChangedEvent != null)
                    {
                        CurrentWaveIndexHasChangedEvent(_currentWaveIndex);
                    }
                }
            }
        }

        /// <summary>
        /// CurrentWaveIndex property backing field
        /// </summary>
        private int _currentWaveIndex = 0;

        void Start()
        {
            // The check that prevents us from making simple mistakes of referencing the same waves and/or appearances 
            SafetyCheck();

            // We initiate all waves at once. Some will just stall until they are started
            foreach (var wave in Waves)
            {
                wave.WaveClearedEvent += CurrentWaveWasCleared;
            }

            // It's already 0, but to be more explicit we set it ourselves
            CurrentWaveIndex = 0;

            // Our current wave index is 0, we start the first wave
            Waves[CurrentWaveIndex].LaunchWave();
        }

        /// <summary>
        /// Helper method that checks whether we are referencing same waves and/or appearances multiple times
        /// </summary>
        private void SafetyCheck()
        {
            // Simple LINQ expression
            // We flatten waves as the sequence of appearances, then group by these appearances
            // If we have more than one item in each group, it means that we've done something wrong
            if (Waves.SelectMany(wave => wave.Appearances).GroupBy(appearance => appearance).Any(group => group.Count() > 1))
            {
                Debug.LogError("Some of the waves are referencing the same appearances or you have the same waves linked to a wave spawner. The game won't work right. Please make sure every wave is referencing a different appearance");
            }
        }

        /// <summary>
        /// Handler method for taking the wave cleared
        /// </summary>
        /// <param name="wave"></param>
        private void CurrentWaveWasCleared(Wave wave)
        {
            PlayerStatus.Instance.Money += wave.CompletionMoney;
            LaunchNextWave(wave);
        }

        private void LaunchNextWave(Wave wave)
        {
            wave.WaveClearedEvent -= LaunchNextWave;

            CurrentWaveIndex++;
            if (CurrentWaveIndex < TotalWavesCount)
            {
                Waves[CurrentWaveIndex].LaunchWave();
            }
            else
            {
                GlobalEventSystem<GameHasEndedEvent>.Raise(new GameHasEndedEvent(false));
            }
        }
    }
}
