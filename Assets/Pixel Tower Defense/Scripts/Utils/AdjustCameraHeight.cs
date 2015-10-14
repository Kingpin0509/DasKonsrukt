using UnityEngine;

namespace Assets.Utils
{
    /// <summary>
    /// Interesting utility class
    /// Handles constant screen width instead of constant screen height
    /// The logic is simple - this script tries to maintain the width 
    /// of the screen in units independent of the aspect ratio
    /// For more info check out this cool talk
    /// http://www.youtube.com/watch?v=rMCLWt1DuqI
    /// </summary>
    [ExecuteInEditMode]
    public class AdjustCameraHeight : MonoBehaviour
    {
        /// <summary>
        /// Editor visible variable that sets the width of the screen in units
        /// </summary>
        public float ActiveZoneWidth = 8f;

        /// <summary>
        /// Cached camera component
        /// </summary>
        private Camera _camera;

        void Awake()
        {
            _camera = GetComponent<Camera>();
            AdjustActiveZoneHeight();
        }

        void Update()
        {
            AdjustActiveZoneHeight();
        }

        public void AdjustActiveZoneHeight()
        {
            float screenAspect = _camera.aspect;

            // Simple formula to calculate the size of the camera
            // the .orthographicSize property is equal to HALF of the screen HEIGHT (that's how Unity works)
            // So all we need to do is calculate the HALF of the screen height based on current aspect ratio 
            // and desired width in units

            _camera.orthographicSize = (1f / screenAspect) * ActiveZoneWidth / 2f;
        }
    }
}
