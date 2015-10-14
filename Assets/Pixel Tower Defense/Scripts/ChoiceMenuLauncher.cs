using Assets.EventSystem;
using Assets.EventSystem.Events;
using Assets.Scripts.Tile;

namespace Assets.Scripts
{
    /// <summary>
    /// A Choice menu launcher
    /// Listens to any tower or empty pad click event, launches the menu and calls the handler for the chosen logic
    /// </summary>
    public class ChoiceMenuLauncher
    {
        /// <summary>
        /// Initialization logic
        /// </summary>
        public static void Initialize()
        {
            // We subscribe to our almighty event
            GlobalEventSystem<TowerOrPadWasPressedEvent>.EventHappened += TowerOrPadWasPressed;  
            // The unsubscribe process is done automatically upon level reloading in the GlobalEventSystem maitenance class
        }

        /// <summary>
        /// Click handler 
        /// Launches the choice menu
        /// </summary>
        /// <param name="args">Event data that is sent along with the event</param>
        private static void TowerOrPadWasPressed(TowerOrPadWasPressedEvent args)
        {
            // Note that we pass a delegate here
            PopupTowerMenu.LaunchMenu(args.SceneItem, ChoiceItemWasPressedHandler);
        }

        /// <summary>
        /// The handling method that calls the appropriate handler of the chosen menu item
        /// </summary>
        /// <param name="item">The item that was picked from the menu</param>
        private static void ChoiceItemWasPressedHandler(ChoiceItem item)
        {
            item.Handler.ProcessTile(item);
        }
    }
}