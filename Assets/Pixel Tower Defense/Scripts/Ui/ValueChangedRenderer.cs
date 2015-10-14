using Assets.EventSystem;
using Assets.EventSystem.Events;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Ui
{
    /// <summary>
    /// Generic class for different kind of value renderers that know how to subscribe to an event of their interest
    /// </summary>
    /// <typeparam name="T">A type of a "value changed" event of interest</typeparam>
    [RequireComponent(typeof(Text))]
    public class ValueChangedRenderer<T> : MonoBehaviour
        where T:IValueChangedEvent
    {
        private Text _textComponent;

        /// <summary>
        /// Common subscription logic
        /// This is where the global event system really shines
        /// </summary>
        private void Awake()
        {
            GlobalEventSystem<T>.EventHappened += ChangeValueText;
            _textComponent = GetComponent<Text>();
        }

        /// <summary>
        /// Value changed handler
        /// Every renderer will behave the same
        /// They only need to update their text
        /// </summary>
        /// <param name="eventArgs">Data that is sent with an event</param>
        private void ChangeValueText(T eventArgs)
        {
            _textComponent.text = eventArgs.GetValue().ToString();
        }
    }
}