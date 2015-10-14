namespace Assets.EventSystem.Events
{
    public struct GameHasEndedEvent
    {
        public readonly bool IsMissionFailed;

        public GameHasEndedEvent(bool isMissionFailed)
        {
            IsMissionFailed = isMissionFailed;
        }
    }
}