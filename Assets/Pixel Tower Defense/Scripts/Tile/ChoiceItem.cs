using System;
using Assets.Scripts.V2;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Tile
{
    /// <summary>
    /// Choice item base class. 
    /// Needed for any choice item, whether it's a tower buy item, an upgrade or a sell choice
    /// </summary>
    public class ChoiceItem : BaseGameObject, IPointerDownHandler
    {
        /// <summary>
        /// Private GameObject field that specifies the actual MonoBehaviour tile
        /// It's marked with a [SerializeField] attribute because we need this field to be private 
        /// AND make it visible in the inspector
        /// Marking a field with this attribute is the easiest way of doing both things
        /// </summary>
        [SerializeField]
        private GameObject _tile;

        /// <summary>
        /// Interesting property
        /// The real tile is fetched with a specific behaviour
        /// Any subclass can override this behaviour and return any other game object 
        /// rather than returning the _tile field
        /// </summary>
        public GameObject Tile { get { return GetTileLogic(); } }

        /// <summary>
        /// Cost of the choice
        /// See _tile field for info about the [SerializeField] attribute
        /// </summary>
        [SerializeField]
        private int _cost;

        /// <summary>
        /// A virtual property of getting the cost. Unity doesn't serialize C# properties correctly
        /// </summary>
        public virtual int Cost { get { return _cost; } set { _cost = value; } }

        /// <summary>
        /// An "utility" property used to link the choice item to it's linkable scene item
        /// </summary>
        public virtual LinkableSceneItem SceneItem { get; set; }

        /// <summary>
        /// The choice also has a TileHandler that specifies how the choice pack should behave once it has been picked
        /// </summary>
        public TileHandler Handler;

        /// <summary>
        /// Public event needed to check which of the specified choice items were pressed
        /// </summary>
        public event Action<ChoiceItem> ChoiceItemPressedEvent;

        /// <summary>
        /// IPointerClickHandler interface implementation
        /// This method is called once the choice item was pressed
        /// </summary>
        /// <param name="eventData">Event data that is passed along with event</param>
        public void OnPointerDown(PointerEventData eventData)
        {
            // These checks actually check whether we have any subscribers
            if (ChoiceItemPressedEvent != null)
            {
                // And then we raise the event
                ChoiceItemPressedEvent(this);
            }
        }

        /// <summary>
        /// Template method of getting the tile game object (or a prefab)
        /// </summary>
        /// <returns>Chosen prefab</returns>
        protected virtual GameObject GetTileLogic()
        {
            // In a base class we just return the _tile field itself
            // Check ValidTowerPoint class for a different implementation
            return _tile;
        }
    }
}