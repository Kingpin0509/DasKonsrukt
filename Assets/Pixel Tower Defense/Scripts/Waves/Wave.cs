using System;
using System.Collections.Generic;
using System.Linq;
using Assets.EventSystem;
using Assets.EventSystem.Events;
using UnityEngine;

namespace Assets.Scripts.Waves
{
    /// <summary>
    /// The Wave class
    /// A manager of appearances - it starts them, checks every dead enemy and fires an event when it has been cleared
    /// </summary>
    public class Wave : MonoBehaviour
    {
        /// <summary>
        /// Editor visible appearances for the wave
        /// </summary>
        public EnemyAppearance[] Appearances;

        /// <summary>
        /// Editor visible competition money field
        /// This value will be granted to a player when this wave is cleared
        /// </summary>
        public int CompletionMoney;

        /// <summary>
        /// An event that fires when all enemies from all appearances (of this waves) are dead
        /// </summary>
        public event Action<Wave> WaveClearedEvent;

        /// <summary>
        /// Utility object that helps to find which enemy resolves to which appearance
        /// </summary>
        private Dictionary<Enemy, EnemyAppearance> _enemyAppearanceReveseLookup;

        /// <summary>
        /// Field used to track current cleared appearances number
        /// </summary>
        private int _clearedAppearances;

        /// <summary>
        /// Public method for launching the wave (called by the WaveSpawner)
        /// </summary>
        public void LaunchWave()
        {
            // Initialization code
            _enemyAppearanceReveseLookup = new Dictionary<Enemy, EnemyAppearance>();
            
            // We will check every dead enemy
            GlobalEventSystem<EnemyDiedEvent>.EventHappened += SpawnedEnemyOnDiedEvent;

            // We start every appearance
            foreach (var enemyAppearance in Appearances)
            {
                enemyAppearance.SpawnedEvent += EnemyAppearanceOnSpawnedEventHandler;
                enemyAppearance.AppearanceCompletedEvent += EnemyAppearanceCompletedEvent;
                enemyAppearance.LaunchSpawning();
            }
        }

        /// <summary>
        /// Appearance completed event
        /// Called when an appearance is cleared
        /// </summary>
        /// <param name="enemyAppearance">The enemy appearance that has been cleared</param>
        private void EnemyAppearanceCompletedEvent(EnemyAppearance enemyAppearance)
        {
            _clearedAppearances++;

            // Unsubscribe for convinience
            enemyAppearance.AppearanceCompletedEvent -= EnemyAppearanceCompletedEvent;
            enemyAppearance.SpawnedEvent -= EnemyAppearanceOnSpawnedEventHandler;

            // We check if this is the last appearance
            if (_clearedAppearances == Appearances.Length)
            {
                // If it is, we do some cleanup
                GlobalEventSystem<EnemyDiedEvent>.EventHappened -= SpawnedEnemyOnDiedEvent;

                // And raise our WaveClearedEvent
                if (WaveClearedEvent != null)
                {
                    WaveClearedEvent(this);
                }
            }
        }

        /// <summary>
        /// An appearance enemy spawned event handler
        /// We spawn our actual enemy here
        /// </summary>
        /// <param name="enemy">A enemy to spawn</param>
        /// <param name="appearance">Appearance that wants to spawn an enemy</param>
        private void EnemyAppearanceOnSpawnedEventHandler(Enemy enemy, EnemyAppearance appearance)
        {
            var spawnedEnemy = Instantiate(enemy);
            _enemyAppearanceReveseLookup[spawnedEnemy] = appearance;
        }

        /// <summary>
        /// Enemy died event handler
        /// We change the number of dead enemies for an appropriate appearance
        /// </summary>
        /// <param name="enemyArgs">An enemy that has died</param>
        private void SpawnedEnemyOnDiedEvent(EnemyDiedEvent enemyArgs)
        {
            if (_enemyAppearanceReveseLookup.ContainsKey(enemyArgs.Enemy))
            {
                var appearance = _enemyAppearanceReveseLookup[enemyArgs.Enemy];
                appearance.NumberOfEnemiesDied++;
            }
        }
    }
}
