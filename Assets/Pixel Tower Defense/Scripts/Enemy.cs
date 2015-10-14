using System;
using System.Collections;
using Assets.EventSystem;
using Assets.EventSystem.Events;
using Assets.Scripts.Towers;
using Assets.Utils;
using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

#endif

namespace Assets.Scripts
{
    /// <summary>
    /// Common enemy class
    /// Represents all mobs in game
    /// </summary>
    public class Enemy : MonoBehaviour
    {
        /// <summary>
        /// Speed of movement
        /// </summary>
        public float Speed = 1f;

        /// <summary>
        /// How much will we give the player for killing this type of mob
        /// </summary>
        public int KillCost = 5;

        /// <summary>
        /// Internal variable representing whether this mob is already dead
        /// </summary>
        private bool _isDead;

        /// <summary>
        /// Public property for specifying whether this mob is dead
        /// </summary>
        public bool IsDead
        {
            get
            {
                // Getting just returns the current state
                return _isDead;
            }
            // We can only set privately
            private set
            {
                _isDead = value;
                // When we set, we check whether this mob is now dead
                if (_isDead)
                {
                    // And if it is, we do our killing routine
                    // We raise a global event
                    GlobalEventSystem<EnemyDiedEvent>.Raise(new EnemyDiedEvent(this));
                    // Adding some money to player
                    PlayerStatus.Instance.Money += KillCost;
                    // Adding some score to player
                    PlayerStatus.Instance.Score += KillCost * 10;
                }
            }
        }

        /// <summary>
        /// Health component
        /// </summary>
        public Health Health { get; set; }

        /// <summary>
        /// Current tile to which we are currently going
        /// </summary>
        private Transform _currentTargetTile;

        /// <summary>
        /// Cached variable to decrease the time of getting the .transform reference. Comes in handy when you have hundreds of enemies
        /// </summary>
        public Transform TransformCache { get; private set; }

        /// <summary>
        /// Current direction of where we are going
        /// </summary>
        private Vector3 _currentDirection;

        /// <summary>
        /// Time to another recalculation of our direction
        /// </summary>
        private float _timeTillRecalculation;

        /// <summary>
        /// Currently passed time since last recalculation
        /// </summary>
        private float _timeTillRecalculationPassed;

        /// <summary>
        /// Cached Animator component
        /// </summary>
        private Animator _animator;

        private void Awake()
        {
            // Getting our components cache
            _animator = GetComponentInChildren<Animator>();
            TransformCache = GetComponent<Transform>();
            Health = GetComponent<Health>();

            // Subscribing to the Health component event, so we can hang our Death logic to it
            Health.HealthChangedEvent += HealthChangedEventHandler;

            // There should be a starting tile in the scene
            var startTile = GameObject.FindGameObjectWithTag(Tags.PathStart);
            // If there's none
            if (startTile == null)
            {
                // Display an error message
                Debug.LogError("Seems that there's no starting point. Please run the tile linking script \n" +
                               "Game Object -> Pixel Tower Defense -> Link Path Tiles");

#if UNITY_EDITOR
                EditorApplication.isPlaying = false;
#endif

                // Flip the table (╯°□°)╯︵ ┻━┻
                return;
            }

            // At the beginning we set the enemy target tile to be the starting tile
            _currentTargetTile = startTile.GetComponent<Transform>();

            // And forse our enemy's position to be this starting tile position
            TransformCache.position = _currentTargetTile.position;

            // Instantly calculating direction to the next tile
            RecalculateTargetTileAndDirection();
        }

        /// <summary>
        /// Health component HealthChanged event handler
        /// Fires when
        /// </summary>
        /// <param name="currentHealth"></param>
        private void HealthChangedEventHandler(float currentHealth)
        {
            if (currentHealth <= 0 && !IsDead)
            {
                Die();
            }
        }

        /// <summary>
        /// Enemy update logic
        /// Responsible for moving the enemy
        /// </summary>
        private void Update()
        {
            // If the enemy is dead, we don't have to process anything
            // This happens when our enemy is playing its "Blow" animation and we don't need it to move or behave
            if (IsDead)
            {
                return;
            }

            // If it's not dead, we recalculate our target and direction
            RecalculateTargetTileAndDirection();

            // And we also rotate our enemy appropriately

            if (_currentDirection.x < -0.5f)
            {
                TransformCache.right = Vector3.left;
            }
            else if (_currentDirection.x > 0.5f)
            {
                TransformCache.right = Vector3.right;                
            }

            // In the end, we move it with the simple transform logic
            TransformCache.position += _currentDirection * Speed * Time.deltaTime;
        }

        /// <summary>
        /// Helper method used to recalculate our current target and direction
        /// It actually recalculates it only once in a while, when we get to our current target
        /// </summary>
        private void RecalculateTargetTileAndDirection()
        {
            //-----------------------
            // Coroutine-like block
            // Simple time counting
            _timeTillRecalculationPassed += Time.deltaTime;
            if (_timeTillRecalculationPassed < _timeTillRecalculation)
            {
                return;
            }

            _timeTillRecalculationPassed = 0f;
            // ----------------------

            // Getting the next time from our current one
            var nextTile = _currentTargetTile.GetComponent<PathTile>().NextTile;

            // If there is one
            if (nextTile != null)
            {
                _currentTargetTile = nextTile.GetComponent<Transform>();

                // Calculating direction to that tile
                var difference = _currentTargetTile.position - TransformCache.position;

                var distanceToNextTile = difference.magnitude;

                _currentDirection = difference.normalized;

                // Time of getting there
                _timeTillRecalculation = distanceToNextTile / Speed;
            }
        }

        /// <summary>
        /// Helper method. Puts all the death logic in one place
        /// </summary>
        private void Die()
        {
            IsDead = true;

            // Play the Blow animation
            _animator.SetTrigger(HashIDs.Die);
            Health.CurrentHealth = 0f;
            _currentDirection = Vector3.zero;

            // Deferred destroy of the object (to let the animation pass)
            // Can be changed to object pooling
            Destroy(gameObject, 0.3f);
        }

        /// <summary>
        /// Hit-The-Base method
        /// Contains logic of hitting the base tower
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter2D(Collider2D other)
        {
            // If we happen to hit anything besides the BaseTower (a Bullet for example), we pass
            if (other.tag != Tags.Base) return;

            // But if it IS the base, we hit it
            
            var baseTower = other.GetComponent<Base>();
            baseTower.HandleEnemyHit();

            // Easiest way not to give player any credits on hit-in-the-base
            KillCost = 0;

            // And die
            Die();
        }
    }
}
