using System.Collections;
using Assets.Scripts.Tile;
using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts.V2
{
    /// <summary>
    /// Interesting class that allows any scene object to be clickable using uGUI canvas logic
    /// On awake (once in a lifetime) it creates a clickable canvas item that is positioned right above the object itself
    /// and is located on a canvas
    /// 
    /// It also contains specific click behaviour
    /// Right now there's only need in Choice Menus, so code is pretty narrow and simple (as it should be)
    /// </summary>
    public class LinkableSceneItem : BaseGameObject
    {
        /// <summary>
        /// Editor linked canvas prefab
        /// </summary>
        public ClickableCanvasItem CanvasItemPrefab;

        /// <summary>
        /// Editor linked choice pack
        /// </summary>
        public ChoicePack ChoicePack;

        /// <summary>
        /// Clickable canvas item cached reference
        /// </summary>
        private ClickableCanvasItem _canvasItemInstance;

        protected override void Awake()
        {
            base.Awake();
            InitializeClickableCanvasItem();
        }

        private void Start()
        {
            // We need to double the initialization because of the strange Unity behaviour that doesn't let the Awake function do all the job
            // Note that we won't create two clickable canvas items due to the check in the initialization function
            InitializeClickableCanvasItem();
        }

        /// <summary>
        /// Canvas item initialization method
        /// It creates an appropriate canvas item if there is none already
        /// </summary>
        private void InitializeClickableCanvasItem()
        {
            // If the canvas item is not specified, we have nothing to instantiate
            if (CanvasItemPrefab == null)
            {
                Debug.LogError("Canvas Item Prefab is not specified for " + GameObjectCache.name);
                return;
            }

            // Finding our current screen position
            var position = Camera.main.WorldToScreenPoint(TransformCache.position);

            // If we haven't already instantiated our canvas item, we instantiate and initialize it
            if (_canvasItemInstance == null)
            {
                _canvasItemInstance = Instantiate(CanvasItemPrefab)
                    as ClickableCanvasItem;
                _canvasItemInstance.SceneItem = this;
            }

            // Resetting the canvas item parent, position and local scale (reinitialization)
            _canvasItemInstance.TransformCache.SetParent(OverlayCanvas.Instance.TransformCache);
            _canvasItemInstance.TransformCache.position = position;
            _canvasItemInstance.TransformCache.localScale = Vector3.one;

            // A simple editor workaround
            // Used to fix the unity issue when using the "Maximise on play" option
            // It happens to maximise the window after a few frames of the game, which results inadequate position of canvas elements on the screen
            // This isn't an issue with a built version because all screen size and resolution changes are handeled nicely
#if UNITY_EDITOR
            StartCoroutine(AdditionalyResetPosition());
#endif
        }

        /// <summary>
        /// Additional destory logic
        /// We also need to destroy our canvas item
        /// </summary>
        public override void Destroy()
        {
            _canvasItemInstance.Destroy();
            base.Destroy();
        }


#if UNITY_EDITOR
        private IEnumerator AdditionalyResetPosition()
        {
            // Skipping two frames
            yield return null;
            yield return null;

            // Resetting the position in case the "Maximise on Play" is being used
            _canvasItemInstance.TransformCache.position = Camera.main.WorldToScreenPoint(TransformCache.position);
            _canvasItemInstance.TransformCache.localScale = Vector3.one;
        }
#endif
    }
}
