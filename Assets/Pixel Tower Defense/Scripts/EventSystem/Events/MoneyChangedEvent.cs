namespace Assets.EventSystem.Events
{
    public struct MoneyChangedEvent : IValueChangedEvent
    {
        private readonly int _value;

        public int GetValue()
        {
            return _value;
        }

        public MoneyChangedEvent(int value)
        {
            _value = value;
        }
    }
}