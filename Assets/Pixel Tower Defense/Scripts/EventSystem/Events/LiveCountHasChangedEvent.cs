namespace Assets.EventSystem.Events
{
    public struct LiveCountHasChangedEvent : IValueChangedEvent
    {
        public readonly int Lives;

        public int GetValue()
        {
            return Lives;
        }

        public LiveCountHasChangedEvent(int lives)
        {
            Lives = lives;
        }
    }
}