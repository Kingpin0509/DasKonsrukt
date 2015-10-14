namespace Assets.EventSystem.Events
{
    public class GamePausedEvent
    {
        public readonly bool IsPaused;

        public GamePausedEvent(bool isPaused)
        {
            IsPaused = isPaused;
        }
    }
}