using System;
using Assets.Scripts.Ui;
using Assets.Scripts.Utils;
using Assets.Scripts.V2;
using Assets.Utils;
using UnityEngine;

namespace Assets.Scripts.Tile
{
    /// <summary>
    /// A popup menu class
    /// Contains logic for displaying the circle choice menu
    /// </summary>
    public class PopupTowerMenu : BaseGameObject
    {
        /// <summary>
        /// Simple singleton logic
        /// </summary>
        private static PopupTowerMenu _popupMenu;

        /// <summary>
        /// Static method to show the menu
        /// </summary>
        /// <param name="sceneItem">Scene (not canvas) item that initiated the call</param>
        /// <param name="callBack">A callback to call when choice happens</param>
        public static void LaunchMenu(LinkableSceneItem sceneItem, Action<ChoiceItem> callBack)
        {
            // If our popupmenu instance is null
            if (_popupMenu == null)
            {
                // We create a game object, name it a Popup Menu
                var newGameObject = new GameObject { name = "Popup Menu" };
                // And we add a Popup Menu component menu
                _popupMenu = newGameObject.AddComponent<PopupTowerMenu>();
            }

            // We then launch a choice menu
            _popupMenu.LaunchChoiceMenu(sceneItem.ChoicePack.ChoiceItems, sceneItem.TransformCache.position,
                callBack, sceneItem);
        }

        /// <summary>
        /// An inner event indicating that no actual choice item was pressed and we instead clicked outside of everything
        /// </summary>
        public event Action ClickedOutsideEvent;

        /// <summary>
        /// The choice items we are currently displaying
        /// </summary>
        private ChoiceItem[] _currentItems;
        
        /// <summary>
        /// The callback action delegate that we are currently ought to call when a user choses something
        /// </summary>
        private Action<ChoiceItem> _currentCallbackAction;

        /// <summary>
        /// Launches the Choice menu
        /// </summary>
        /// <param name="choiceItems">Choice items to show</param>
        /// <param name="position">Current WORLD position to display the menu</param>
        /// <param name="callback">Callback to call when something gets picked</param>
        /// <param name="sceneItem">Scene item that initiated the click</param>
        public void LaunchChoiceMenu(ChoiceItem[] choiceItems, Vector3 position,
            Action<ChoiceItem> callback, LinkableSceneItem sceneItem)
        {
            // Setting our instance delegate to current callback so we will be able to call it later
            _currentCallbackAction = callback;

            // Setting our gameObject cache to true
            base.GameObjectCache.SetActive(true);

            // We also enable our click blocker
            // that transparent light panel
            UiClickBlocker.Instance.Enable();
            // And set it to the top "layer" so it covers everyting (except the actual menu that we will add later)
            UiClickBlocker.Instance.TransformCache.SetAsLastSibling();

            // We then recalculate our position and get actual screen point in pixels
            position = Camera.main.WorldToScreenPoint(position);
            // And set this menu to this point
            TransformCache.position = position;

            // Sometimes it's not obvious, but we also need to set this gameObject as a child of the Canvas
            // So it will be rendered properly
            TransformCache.SetParent(OverlayCanvas.Instance.TransformCache);
            // And set it to the top "layer" so it covers everyting
            TransformCache.SetAsLastSibling();

            // Calculating positions of the choice items
            // They should uniformly fill the 360 circle
            int totalChoiceCount = choiceItems.Length;
            // Actual angle step is calculated by dividing the 2 PI (360 degrees) by total count of items
            float angleStep = 2f * Mathf.PI / totalChoiceCount;
            // Scale to step items from the center
            const float scale = 45f;

            // Creating a local array of actual choice items
            _currentItems = new ChoiceItem[totalChoiceCount];

            // For every passed choice item
            for (int i = 0; i < totalChoiceCount; i++)
            {
                var currentChoiceItem = choiceItems[i];
                // If we're given a prefab (that's what .activeInHierarchy means)
                if (!currentChoiceItem.gameObject.activeInHierarchy)
                {
                    // We instantiate it first
                    // Destory and Instantiate things can be easily rewired to some object pool logic
                    currentChoiceItem = Instantiate(currentChoiceItem, position, Quaternion.identity) as ChoiceItem;
                }

                // We link this choice item to a passed scene item
                currentChoiceItem.SceneItem = sceneItem;
                // Set it as a child of the canvas (it's an UI.Image)
                currentChoiceItem.transform.SetParent(OverlayCanvas.Instance.TransformCache);
                // And set it to the top of everything else
                currentChoiceItem.transform.SetAsLastSibling();

                // We find the X and Y positions of the item
                var rotationVector = new Vector3
                {
                    x = Mathf.Sin(angleStep * i) * scale,
                    y = Mathf.Cos(angleStep * i) * scale
                };

                // Change the position of the element itself
                currentChoiceItem.transform.position += rotationVector;

                // And subscribe to it's Pressed event so we are able to catch the item that was chosen
                currentChoiceItem.ChoiceItemPressedEvent += ChoiceItemWasPressedEventHandler;
                
                // Finally, we store newly created item in our array
                _currentItems[i] = currentChoiceItem;
            }

            // After all is done, we're just waiting for either any of the choices to fire a ChoiceItemPressedEvent event,
            // or a click blocker to fire its BlockerWasClickedEvent event
        }

        /// <summary>
        /// Handler method that is getting called when any of the choice items gets pressed
        /// </summary>
        /// <param name="choiceItem">The choice item that was pressed</param>
        private void ChoiceItemWasPressedEventHandler(ChoiceItem choiceItem)
        {
            // If our current callback action is not null
            // AND if player has enough money
            if (_currentCallbackAction != null && choiceItem.Cost <= PlayerStatus.Instance.Money)
            {
                // We call the callback action
                _currentCallbackAction(choiceItem);

                // And clearing the menu
                ClearMenu();
            }
        }

        /// <summary>
        /// Automatic Unity method in which we subscribe to the click blocker press event so we can see
        /// whether none of our choice items was pressed
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            UiClickBlocker.Instance.BlockerWasClickedEvent += InstanceOnBlockerWasClickedEventHandler;
        }

        /// <summary>
        /// Handling logic for ClickBlocker click
        /// </summary>
        private void InstanceOnBlockerWasClickedEventHandler()
        {
            // We disable the click blocker
            UiClickBlocker.Instance.Disable();
            if (ClickedOutsideEvent != null)
            {
                // And call our current callback action with the null param, indicating that none of the choices was picked
                _currentCallbackAction(null);
            }

            // Then we clear our menu
            ClearMenu();
        }

        /// <summary>
        /// Logic for clearing the menu
        /// </summary>
        private void ClearMenu()
        {
            // We clean every choice item
            for (int i = 0; i < _currentItems.Length; i++)
            {
                // Destory and Instantiate things can be easily rewired to some object pool logic
                Destroy(_currentItems[i].gameObject);
            }

            // Disable our current popup window
            GameObjectCache.SetActive(false);
            // And disable the click blocker (if it hasn't been already)
            UiClickBlocker.Instance.Disable();

            // We also drop our current items array
            // By default you maintain references to destroyed Unity objects
            _currentItems = null;
        }
    }
}