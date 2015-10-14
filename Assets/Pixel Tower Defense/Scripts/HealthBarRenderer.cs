using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    /// <summary>
    /// Renderer component for health
    /// </summary>
    public class HealthBarRenderer : MonoBehaviour
    {
        /// <summary>
        /// Linked health component
        /// </summary>
        private Health _healthScript;

        /// <summary>
        /// Rect transfrom of the mask. Used to control the degree of health fill
        /// </summary>
        private RectTransform _maskRectTransform;

        /// <summary>
        /// Maximum width of the renderer. Used to calculate current width
        /// </summary>
        private float _maxWidth;

        /// <summary>
        /// Initialization method
        /// </summary>
        void Start()
        {
            // We assume that the Health component is on our parent
            _healthScript = GetComponentInParent<Health>();
            // We get it and we subscribe to its health changed event, so we know when to update current width
            _healthScript.HealthChangedEvent += HealthChangedEventListener;

            // We also find our masking component
            _maskRectTransform = GetComponentInChildren<Mask>().GetComponent<RectTransform>();
            _maxWidth = _maskRectTransform.rect.width;
        }

        /// <summary>
        /// Health changed event handler
        /// We change the width of the mask component in it
        /// </summary>
        /// <param name="currentHealth">New health value</param>
        private void HealthChangedEventListener(float currentHealth)
        {
            // We calculate the width ratio and set it as the size delta of our mask
            _maskRectTransform.sizeDelta = new Vector2(_healthScript.CurrentHealthRatio * _maxWidth,
                _maskRectTransform.rect.height);

            // If new health is less than zero, we need to make sure that the health element will be destroyed
            // along with the enemy itself
            // Right now it will be destroyed anyway, but if it won't be linked to the enemy, there will be a problem
            // so we need this additional destroy logic
            if (currentHealth <= 0)
            {
                _healthScript.HealthChangedEvent -= HealthChangedEventListener;
                Destroy(gameObject);
            }
        }
    }
}
