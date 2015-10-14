using Assets.EventSystem;
using Assets.EventSystem.Events;

namespace Assets.Utils
{
    /// <summary>
    /// Helper class that handles current player score, money and lives
    /// It's made using a simple singleton manner
    /// </summary>
    public class PlayerStatus
    {
        private static PlayerStatus _instance;

        public static PlayerStatus Instance
        {
            get { return _instance ?? (_instance = new PlayerStatus()); } 
        }

        private PlayerStatus(){}

        private int _money;
        public int Money
        {
            get { return _money; }
            set
            {
                _money = value;
                // When we set our property, we also fire an event
                GlobalEventSystem<MoneyChangedEvent>.Raise(new MoneyChangedEvent(_money));
            } 
        }

        private int _score;
        public int Score
        {
            get { return _score; }
            set
            {
                _score = value;
                // When we set our property, we also fire an event
                GlobalEventSystem<ScoreChangedEvent>.Raise(new ScoreChangedEvent(_score));
            }
        }

        private int _lives;
        public int Lives
        {
            get { return _lives; }
            set
            {
                _lives = value;
                // When we set our property, we also fire an event
                GlobalEventSystem<LiveCountHasChangedEvent>.Raise(new LiveCountHasChangedEvent(_lives));
            }
        }

        public int MaxLives { get; set; }
    }
}