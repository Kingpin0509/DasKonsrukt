using Assets.Scripts;
using Assets.Scripts.Ui;
using UnityEngine;


/// <summary>
/// Helper class that contains logic for globally visible common canvas
/// It's used to add new uGUI items to the screen
/// </summary>
public class OverlayCanvas : BaseGameObject
{

    /// <summary>
    /// Simple singleton logic with a bit of performance tweaks
    /// </summary>
    public static OverlayCanvas Instance
    {
        get
        {
            // Helper boolean field
            // Used to avoid calling GameObject.FindObjectOfType function
            if (!_isAlreadyAssigned)
            {
                _overlayCanvas = GameObject.FindObjectOfType<OverlayCanvas>();
                if (_overlayCanvas == null)
                {
                    Debug.LogError("Overlay canvas has not been found. You either disabled it in the inspector or didn't setup the level properly");
                    return null;
                }

                // _isAlreadyAssigned boolean is set to "true" there;
                _overlayCanvas.Awake();
            }

            return _overlayCanvas;
        }

        private set
        {
            _overlayCanvas = value;
            _isAlreadyAssigned = true;
        }
    }

    /// <summary>
    /// Editor linked UiClickBlocker game object
    /// </summary>
    public UiClickBlocker ClickBlocker;

    /// <summary>
    /// Some performance tweaks. It is false until our _overlayCanvas is null
    /// </summary>
    private static bool _isAlreadyAssigned = false;

    /// <summary>
    /// Static property backing field
    /// </summary>
    private static OverlayCanvas _overlayCanvas;

    /// <summary>
    /// Shortcut canvas Transform property
    /// </summary>
    public static Transform Transform { get { return Instance.TransformCache; } }

    protected override void Awake()
    {
        base.Awake();
        Instance = this;
        ClickBlocker.gameObject.SetActive(true);
        ClickBlocker.gameObject.SetActive(false);
    }
}

