using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.EventSystem;
using Assets.EventSystem.Events;
using Assets.Scripts.V2;
using UnityEngine;

namespace Assets.Scripts.Towers
{
    /// <summary>
    /// Class representing the actual towers that target and shoot enemies
    /// </summary>
    [RequireComponent(typeof(CircleCollider2D))]
    public class Tower : BaseGameObject
    {
        /// <summary>
        /// Event of changing the rank
        /// Usually used by rank renderer
        /// </summary>
        public event Action<int> RankedHasChangedEvent;

        /// <summary>
        /// Rank property logic
        /// We fire an event if a new rank is different
        /// </summary>
        public int Rank
        {
            get
            {
                return _rank;
            }
            set
            {
                // We do nothing if our new rank is the same
                if (_rank == value) return;

                _rank = value;

                UpdateNewRankRadius();
                // We also fire an event if we have any subscribers
                if (RankedHasChangedEvent != null)
                {
                    RankedHasChangedEvent(_rank);
                }
            }
        }

        

        /// <summary>
        /// Tower preferences object
        /// All the tower prefs are stored there
        /// </summary>
        public TowerPrefs TowerPreferences;

        /// <summary>
        /// Bullet prefabs that is linked to this tower
        /// </summary>
        public Bullet BulletPrefab;

        /// <summary>
        /// Optional parameter specifying the bullet firing point
        /// If it's not set, the bullet will fore from the center of the tower
        /// </summary>
        public Transform BulletSpawnPoint;

        /// <summary>
        /// Rank backing field (to use with the Rank property)
        /// </summary>
        [SerializeField]
        private int _rank = 1;

        /// <summary>
        /// Cached Animator reference
        /// </summary>
        private Animator _towerAnimator;

        /// <summary>
        /// Possible enemies inside tower's range
        /// </summary>
        private List<Enemy> _targetsInRange;

        /// <summary>
        /// Currently active target. Any tower can target only one enemy at a time
        /// </summary>
        private Enemy _currentTarget;

        private CircleCollider2D _circleCollider;

        /// <summary>
        /// Firing cooldown time
        /// Can be also done with a coroutine
        /// </summary>
        private float _currentWaitToFireTime;

        /// <summary>
        /// Time to hold the firing "animation"
        /// To avoid too fast blinking
        /// </summary>
        private const float FiringLength = 0.3f;

        /// <summary>
        /// Simple initialization method
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            _towerAnimator = GetComponentInChildren<Animator>();
            _circleCollider = GetComponentInChildren<CircleCollider2D>();
            
            _targetsInRange = new List<Enemy>();
            // Subscribing to a global enemy died event
            GlobalEventSystem<EnemyDiedEvent>.EventHappened += EnemyDiedEventHandler;
        }

        /// <summary>
        /// Method with common logic
        /// We handle firing here
        /// </summary>
        private void Update()
        {
            // If the tower doesn't have a target, simply ignore all the logic untill we have one
            if (_currentTarget == null) return;

            // If we have a target, we check the firing rate
            _currentWaitToFireTime += Time.deltaTime;

            // We check the direction to the enemy so the tower can point the gun appropriately 
            var difference = _currentTarget.TransformCache.position
                - TransformCache.position;
            
            // Getting the angle
            var angle = Mathf.Atan2(difference.x, difference.y) * Mathf.Rad2Deg;

            // Feeding the angle into the tower animator
            _towerAnimator.SetFloat(HashIDs.Angle, angle);

            // Now we check the firing time
            // If it's time to fire
            if (_currentWaitToFireTime > TowerPreferences[_rank].FireRate)
            {
                // We first updating the animator
                _towerAnimator.SetBool(HashIDs.Fire, true);
                // Resetting the firing time
                _currentWaitToFireTime = 0f;

                // And fire our bullet
                if (BulletPrefab != null)
                {
                    // It's here that we can use the BulletSpawnPoint position
                    // If it's null, we just fire from the center of the tower
                    var spawnPosition = BulletSpawnPoint != null ? BulletSpawnPoint.position : TransformCache.position;

                    // We also instantiate the bullet prefab
                    // It can be done using object pool also
                    var bullet = Instantiate(BulletPrefab, spawnPosition, Quaternion.identity) as Bullet;

                    // Bullet prefs
                    bullet.Target = _currentTarget;
                    bullet.Damage = TowerPreferences[_rank].Damage;
                }

                // We also start a coroutine to reset the tower sprite
                StartCoroutine(ResetFiringState());
            }
        }

        /// <summary>
        /// Trigger Enter logic
        /// We check whether we have a target, and a newly entered entity is enemy
        /// If we have a target, we just add it to the list
        /// If not, we just set our target to that entity
        /// </summary>
        /// <param name="other">Entering entity</param>
        private void OnTriggerEnter2D(Collider2D other)
        {
            var possibleEnemy = other.GetComponent<Enemy>();
            if (possibleEnemy == null) return;

            // We only get here if the entering entity is an Enemy

            // If we don't have a target
            if (_currentTarget == null)
            {
                // We just set this entity as our target
                ChangeCurrentTargetTo(possibleEnemy);
            }
            else
            {
                // Or we add it to the possible enemies list
                _targetsInRange.Add(possibleEnemy);
            }
        }

        /// <summary>
        /// Simple cleanup code
        /// </summary>
        private void OnDestroy()
        {
            GlobalEventSystem<EnemyDiedEvent>.EventHappened -= EnemyDiedEventHandler;
        }

        /// <summary>
        /// Common logic for enemy died event
        /// We check if that enemy is our target. If it is, we choose another one
        /// </summary>
        /// <param name="enemyArgs"></param>
        private void EnemyDiedEventHandler(EnemyDiedEvent enemyArgs)
        {
            var enemy = enemyArgs.Enemy;

            // If this dead enemy was in our possible enemies list, we remove it
            if (_targetsInRange.Contains(enemy))
            {
                _targetsInRange.Remove(enemy);
            }

            // If this dead enemy was our target, we choose a new one from the list
            if (_currentTarget == enemy)
            {
                ChangeCurrentTargetTo(ChooseFromTargetsInRange());
            }
        }

        /// <summary>
        /// Logic of the entity leaving the radius
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerExit2D(Collider2D other)
        {
            var possibleEnemy = other.GetComponent<Enemy>();
            if (possibleEnemy == null) return;

            // Removing the leaving enemy from possible enemies list
            _targetsInRange.Remove(possibleEnemy);

            // If it's our current target, we have to choose a new one
            if (_currentTarget == possibleEnemy)
            {
                ChangeCurrentTargetTo(ChooseFromTargetsInRange());
            }
        }

        /// <summary>
        /// Helper method of changing our current target
        /// Made to reserve some additional logic
        /// </summary>
        /// <param name="enemy">Enemy to change our target to</param>
        private void ChangeCurrentTargetTo(Enemy enemy)
        {
            _currentTarget = enemy;
        }

        /// <summary>
        /// Helper method that takes the closest enemy from possible enemies list
        /// </summary>
        /// <returns></returns>
        private Enemy ChooseFromTargetsInRange()
        {
            // Simple LINQ expression
            // It makes a bit of allocations
            // It was not an issue up to date, but it's possible to rewrite this to zero allocations
            var bestMatchTarget = _targetsInRange
                // Safeguard from MissingReferenceException
                .Where(x => x.TransformCache != null)
                // First, we order all enemies by distance
                .OrderBy(x => (x.TransformCache.position - TransformCache.position).sqrMagnitude)
                // Then return the closest enemy
                .FirstOrDefault();

            // One can write "return" directly in the LINQ expression without this local variable
            // It's here to help with possible debugging
            return bestMatchTarget;
        }

        /// <summary>
        /// Almost all needed prefs and info is stored inside the TowerPreferences object
        /// But we still need to update the Tower's collider radius
        /// </summary>
        private void UpdateNewRankRadius()
        {
            _circleCollider.radius = TowerPreferences[Rank].Range;
        }

        /// <summary>
        /// Coroutine to reset the firing state. This is what holds the "Firing" sprite on the tower
        /// </summary>
        /// <returns></returns>
        private IEnumerator ResetFiringState()
        {
            yield return new WaitForSeconds(FiringLength);
            _towerAnimator.SetBool(HashIDs.Fire, false);
        }
    }
}
