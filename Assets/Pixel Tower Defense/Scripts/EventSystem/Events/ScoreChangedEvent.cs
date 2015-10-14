namespace Assets.EventSystem.Events
{
    public class ScoreChangedEvent : IValueChangedEvent
    {
        private readonly int _score;

        public ScoreChangedEvent(int score)
        {
            _score = score;
        }

        public int GetValue()
        {
            return _score;
        }
    }
}