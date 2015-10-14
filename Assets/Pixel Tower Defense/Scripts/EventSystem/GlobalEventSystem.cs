using System;
using System.Collections.Generic;

//----------------------------
// Some interesting stuff is going on here
// We have a non-generic abstract base class which serves only the purpose of storing all the generic
// subclasses inside a common list (you'll see one in the GlobalEventSystemMaitenance class).
// Different events inside the EventSystem.Events namespace (and folder) are just statically typed 
// entities used to differentiate the events one from another
//----------------------------

namespace Assets.EventSystem
{
    /// <summary>
    /// Common base class for our generic event system
    /// </summary>
    public abstract class GlobalEventSystem
    {
        /// <summary>
        /// The only method is supposed to be implemented the same, 
        /// but we need to defer that behaviour to subclasses
        /// </summary>
        public abstract void CleanEventSystem();
    }

    /// <summary>
    /// The actual generic event system class via which you can raise and get events and pass data
    /// </summary>
    /// <typeparam name="T">The type of the event for this system</typeparam>
    public class GlobalEventSystem<T> : GlobalEventSystem
    {
        //---------------------
        // Static content
        // --------------------

        /// <summary>
        /// The backing delegate for our event
        /// We need it because our subscribe logic is different from the standard .NET implementation
        /// </summary>
        private Action<T> _eventBackingDelegate;

        /// <summary>
        /// Static event which is visible throughout the game
        /// Every closed generic type will have its own backing delegate field 
        /// because the compiler treats generic types as if they were completely different types
        /// </summary>
        public static event Action<T> EventHappened
        {
            // The add logic of the event. It's triggered by the += command
            add
            {
                // If it's our first subscriber
                if (EventSystemInstance._eventBackingDelegate == null)
                {
                    // We register this (newly) closed generic class in the Maitenance class
                    // So we can easily clean it up later
                    GlobalEventSystemMaitenance.RegisterNewEventSystem(EventSystemInstance);
                }

                // And of cource we subscribe the provided method to this particular event
                EventSystemInstance._eventBackingDelegate += value;
            }
            // The remove logic of the event. It's triggered by the -= command
            // We unsibscribe the subscribers here
            remove { EventSystemInstance._eventBackingDelegate -= value; }
        }

        /// <summary>
        /// Static method to raise the event
        /// It's an open discussion whether we need to pass data or should it be just the event itself
        /// One could also add a Sender object param, but it can be passed along with the event data
        /// It's just a matter of taste
        /// </summary>
        /// <param name="eventData">The event data to pass along with the event</param>
        public static void Raise(T eventData)
        {
            // Reserved logic for the safe raise. For now, the only possible issue is to stumble upon a null delegate
            // Also notice - it's an instance method
            EventSystemInstance.SafeRaise(eventData);
        }

        /// <summary>
        /// Very important method. It cleans up current event system
        /// It unsubscribes every subscriber and sets the static instance to null
        /// So the next time you try to subscribe to the event, you'll se a completely new event system
        /// Usualli this happens when you load a new level or your game is over
        /// </summary>
        private static void CleanCurrentEventSystem()
        {
            // If our instance is not null
            if (_eventSystemInstance != null)
            {
                _eventSystemInstance.CleanSubscribersList();
                // We set our istance to null, so we can check whether we have to create a new instance next time
                _eventSystemInstance = null;
            }
        }

        /// <summary>
        /// The singleton instance
        /// The "singleton" word here is not appropriate though, 
        /// we will have an instance for every different T we get
        /// </summary>
        private static GlobalEventSystem<T> _eventSystemInstance;

        /// <summary>
        /// The getter property for the static instance. Simple logic of "Create If Null"
        /// </summary>
        private static GlobalEventSystem<T> EventSystemInstance
        {
            // This strange ?? operator actually wraps up the following code:
            //
            // if(_eventSystemInstance != null)
            // {
            //     return _eventSystemInstance;
            // }
            // else
            // {
            //     _eventSystemInstance = new GlobalEventSystem<T>();
            //     return _eventSystemInstance;
            // }

            get { return _eventSystemInstance ?? (_eventSystemInstance = new GlobalEventSystem<T>()); }
        }

        //---------------------
        // Instance content
        // --------------------

        /// <summary>
        /// Implementation an abstract method
        /// </summary>
        public override void CleanEventSystem()
        {
            // Notice that we call a static method here
            GlobalEventSystem<T>.CleanCurrentEventSystem();
        }

        /// <summary>
        /// Logic of cleaning the subscrubers list
        /// It's a simple delegate, so we can set it to null
        /// </summary>
        private void CleanSubscribersList()
        {
            _eventBackingDelegate = null;
        }

        /// <summary>
        /// Helper method to safe raise an event
        /// </summary>
        /// <param name="eventData">Data to pass along with the event</param>
        private void SafeRaise(T eventData)
        {
            if (_eventBackingDelegate != null)
            {
                _eventBackingDelegate(eventData);
            }
        }

        /// <summary>
        /// Private constructor so we can only instantiate this class from within itself
        /// </summary>
        private GlobalEventSystem()
        {
        }
    }

    /// <summary>
    /// A maitenance class used to cleanup every used event system
    /// We need these three classes because of the interesting C# delegate world
    /// </summary>
    public static class GlobalEventSystemMaitenance
    {
        /// <summary>
        /// Our allmighty list of all used event systems
        /// It gets filled up when game objects subscribe to new kinds of events
        /// Important thing to notice - it will have as many items as there are different kinds of events 
        /// that game object has been subscribed to.
        /// If there are 1000 objects subscribed to some PlayerDied event 
        /// (and there are no subscribers to any different kind of event), 
        /// there will be only one item in this list
        /// </summary>
        private static readonly List<GlobalEventSystem> GlobalEventSystems = new List<GlobalEventSystem>();

        /// <summary>
        /// Registers a newly "initialized" event system
        /// We can add different closed generic types because they all share the same superclass
        /// </summary>
        /// <param name="eventSystem">The event system to register</param>
        public static void RegisterNewEventSystem(GlobalEventSystem eventSystem)
        {
            // Just a double check
            // This statement would never be false because each event system only registers itself once
            // but you know, you can never be absolutely bulletproof

            // If we don't have this event system in our list yet
            if (!GlobalEventSystems.Contains(eventSystem))
            {
                // Only then we add it to the list
                GlobalEventSystems.Add(eventSystem);
            }
        }

        /// <summary>
        /// The magic method that actually cleans up every event system that has been noticed
        /// </summary>
        public static void CleanupEventSystem()
        {
            // For every registered event system
            foreach (var globalEventSystem in GlobalEventSystems)
            {
                // We let it clean itself of the subscribers and stuff
                globalEventSystem.CleanEventSystem();
            }

            // And clear our list, so it becomes fresh and empty
            GlobalEventSystems.Clear();
        }
    }
}