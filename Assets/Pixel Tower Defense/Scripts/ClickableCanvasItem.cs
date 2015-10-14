using Assets.EventSystem;
using Assets.EventSystem.Events;
using Assets.Scripts.V2;
using UnityEngine.EventSystems;

namespace Assets.Scripts
{
    /// <summary>
    /// A class to allow any scene object be clicked using uGUI canvas logic
    /// Every clickable canvas item is created automatically and it's screen position is specified by the 
    /// scene item (using world - to - screen position transformation)
    /// </summary>
    public class ClickableCanvasItem : BaseGameObject, IPointerDownHandler
    {
        /// <summary>
        /// Linked scene item
        /// Tower or a empty pad for example
        /// </summary>
        public LinkableSceneItem SceneItem { get; set; }

        /// <summary>
        /// Pointer down handler
        /// Contains logic for click processing
        /// </summary>
        /// <param name="eventData">Data that is sent along with the event</param>
        public void OnPointerDown(PointerEventData eventData)
        {
            // If this scene item is linked to anything
            if(SceneItem != null)
            {
                // We raise a global event, saying that this canvas item was pressed

                // This logic may seem to be a bit strange, but right now our clickable items are only the towers and empty tower pads
                // And the only thing we do to them is opening the choice menu
                // Even if we would tell the linked scene item about the click, we will have to open the menu somehow
                // So this is the simplest possible solution which requires less call nesting

                GlobalEventSystem<TowerOrPadWasPressedEvent>.Raise(
                new TowerOrPadWasPressedEvent(SceneItem));
            }
        }
    }
}
