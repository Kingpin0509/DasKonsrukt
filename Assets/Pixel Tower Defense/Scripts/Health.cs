using System;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// Health class (usually used by enemies)
    /// </summary>
    public class Health : MonoBehaviour
    {
        /// <summary>
        /// Maximum health of the entity
        /// It's private, but editor visible
        /// </summary>
        [SerializeField]
        private float _maxHealth = 100f;

        /// <summary>
        /// Current health backing field
        /// </summary>
        private float _currentHealth;

        /// <summary>
        /// Current health property
        /// Contains additional logic of raising an event upon changing this property
        /// </summary>
        public float CurrentHealth
        {
            get { return _currentHealth; }

            set
            {
                _currentHealth = value;

                if (HealthChangedEvent != null)
                {
                    HealthChangedEvent(_currentHealth);
                }
            }
        }

        /// <summary>
        /// Helper property
        /// Used to easily find current health ratio
        /// </summary>
        public float CurrentHealthRatio
        {
            get { return CurrentHealth / _maxHealth; }
        }

        /// <summary>
        /// Event that fires when the health changes
        /// </summary>
        public event Action<float> HealthChangedEvent;

        /// <summary>
        /// Simple initialization
        /// </summary>
        void Start()
        {
            CurrentHealth = _maxHealth;
        }
    }
}