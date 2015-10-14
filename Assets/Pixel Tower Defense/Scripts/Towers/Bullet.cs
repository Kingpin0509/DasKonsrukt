using UnityEngine;

namespace Assets.Scripts.Towers
{
    /// <summary>
    /// Simple bullet class
    /// Contains logic for moving a bullet
    /// </summary>
    public class Bullet : MonoBehaviour
    {
        /// <summary>
        /// Editor visible speed of the bullet field
        /// Specifies the speed at which the bullet travels
        /// </summary>
        public float Speed = 3f;

        /// <summary>
        /// Enemy target property
        /// Initialized when the bullet is created (or reinitialized)
        /// It's the target to which the bullet will be moving
        /// </summary>
        public Enemy Target { get; set; }

        /// <summary>
        /// The damage of the bullet depends on the Tower that shot the bullet
        /// </summary>
        public float Damage { get; set; }

        /// <summary>
        /// Our cached transform reference
        /// </summary>
        private Transform _transformCache;

        /// <summary>
        /// Cached animator reference
        /// </summary>
        private Animator _animator;

        /// <summary>
        /// Last seen position of our enemy
        /// It's used when an enemy is killed by another bullet
        /// The bullet that is still targeting that enemy will fly till it reaches that point and then explode
        /// </summary>
        private Vector3? _lastSeenPosition;

        /// <summary>
        /// Simple setup code
        /// </summary>
        private void Start()
        {
            _transformCache = GetComponent<Transform>();
            _animator = GetComponent<Animator>();
        }

        /// <summary>
        /// Main logic method
        /// We move to the enemy and hit it if we're close enough
        /// </summary>
        private void Update()
        {
            // If we have no target, or no last seen position, we simply return (maybe we should destroy this bullet)
            if (Target == null || Target.TransformCache == null && _lastSeenPosition == null) return;

            Vector3 targetPosition;
            
            // If the enemy is still alive, it's transform component won't be null
            if (Target.TransformCache != null)
            {
                // We set our current target position to the enemy position
                targetPosition = Target.TransformCache.position;
                // And store that position as last seen position
                _lastSeenPosition = targetPosition;
            }
            // If the enemy is already dead
            else
            {
                // We set our current target position to the last seen position
                targetPosition = _lastSeenPosition.Value;
            }

            // We find our current movement direction
            var dir = (targetPosition - _transformCache.position);
            
            // Move the bullet towards the enemy
            _transformCache.position += dir.normalized * Speed * Time.deltaTime;

            // And rotate the bullet accordingly
            _transformCache.right = -dir;

            // If the sqrMagnitude is less than 0.01 (magnitude is less than 0.1)
            // we consider it as hit
            if (dir.sqrMagnitude < 0.01f)
            {
                // If we still have a target (it might be dead already, check the code above)
                if (Target != null)
                {
                    // We decrease its health
                    Target.GetComponent<Health>().CurrentHealth -= Damage;
                    // And consider we've done our job
                    Target = null;
                }

                // Resetting everything
                _lastSeenPosition = null;
                // Countdown to destroy (possible to move it to object pooling)
                Destroy(gameObject, 1f);
                // And set our animator to blow
                _animator.SetTrigger(HashIDs.Blow);
            }
        }
    }
}
