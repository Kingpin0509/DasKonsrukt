using System;
using System.Collections;
using Assets.EventSystem;
using Assets.EventSystem.Events;
using Assets.Scripts;
using UnityEngine;

/// <summary>
/// Enemy appearance class
/// Contains logic about which type of enemy to spawn, how many and at which rate
/// It doesn't contain logic for spawning itself, it just tells it's Wave which enemy to spawn and when
/// The spawning itself is done in Wave class
/// </summary>
public class EnemyAppearance : MonoBehaviour
{
    /// <summary>
    /// Editor linked enemy prefab
    /// </summary>
    public Enemy Enemy;

    /// <summary>
    /// Editor visible spawn rate field
    /// </summary>
    public float SpawnRate;

    /// <summary>
    /// Editor visible number of enemies field
    /// </summary>
    public int NumberOfEnemies;

    /// <summary>
    /// Shortcut property for getting the number of enemies that already died
    /// with custom set logic (event dispatching)
    /// </summary>
    public int NumberOfEnemiesDied
    {
        get { return _enemiesDied; }
        set
        {
            _enemiesDied = value;

            if (_enemiesDied == NumberOfEnemies)
            {
                AppearanceCompletedEvent(this);
            }
        }
    }

    /// <summary>
    /// Property backing field
    /// </summary>
    private int _enemiesDied;

    /// <summary>
    /// Cached WaitForSeconds object
    /// Spawn rate is constant so we can create only one object for entire appearance
    /// </summary>
    private WaitForSeconds _waitTime;

    /// <summary>
    /// Spawn event, fires when a new enemy is spawned
    /// </summary>
    public event Action<Enemy, EnemyAppearance> SpawnedEvent;

    /// <summary>
    /// Completed event, fires when all possible enemies are spawned
    /// </summary>
    public event Action<EnemyAppearance> AppearanceCompletedEvent;

    /// <summary>
    /// Launch spawning method (called by the Wave Spawner)
    /// Starts the appearance spawning process
    /// </summary>
    public void LaunchSpawning()
    {
        _waitTime = new WaitForSeconds(SpawnRate);

        StartCoroutine(Spawn());
    }

    /// <summary>
    /// Spawn coroutine
    /// Simple logic of enemies spawning
    /// </summary>
    IEnumerator Spawn()
    {
        // We spawn the specified number of enemies, also firing our event
        for (int enemiesSpawned = 0; enemiesSpawned < NumberOfEnemies; enemiesSpawned++)
        {
            yield return _waitTime;
            if (SpawnedEvent != null)
            {
                SpawnedEvent(Enemy, this);
            }
        }
    }
}
