using UnityEngine;

namespace Assets.Scripts.Utils
{
    /// <summary>
    /// Common sprite Z sorter. Takes care of ordering sprites on the screen.
    /// Distinguishes between static sprites (the order of which only calculates once) 
    /// and dynamic sprites which have a tendency to move
    /// For more info check this cool talk
    /// http://www.youtube.com/watch?v=rMCLWt1DuqI
    /// </summary>
    [ExecuteInEditMode]
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteSorterZ : MonoBehaviour
    {
        /// <summary>
        /// Specifies whether the sprite we're reordering is static
        /// </summary>
        public bool IsStatic;

        /// <summary>
        /// Cached link to our SpriteRenderer component
        /// </summary>
        private SpriteRenderer _spriteRenderer;

        /// <summary>
        /// Cached link to our Transform component
        /// </summary>
        private Transform _transform;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _transform = GetComponent<Transform>();

            // Recalculating at least once in a lifetime of an object
            RecalculateSortingOrder();
        }

        private void Update()
        {
            // If our object is static, we don't need to recalculate anything
            if (!IsStatic)
            {
                // If it's dynamic - recalculate
                RecalculateSortingOrder();
            }
        }

        /// <summary>
        /// Simple way of ordering sprites in their layers
        /// We take their position in the world and multiply it by an arbitrary value (has to be chosen for specific needs)
        /// </summary>
        private void RecalculateSortingOrder()
        {
            // Changing sorting order based on Y coordinate of our sprite's position
            _spriteRenderer.sortingOrder = -(int)(_transform.position.y * 10f);            
        }
    }
}