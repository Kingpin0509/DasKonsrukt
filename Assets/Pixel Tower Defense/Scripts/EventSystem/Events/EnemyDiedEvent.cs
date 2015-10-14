using Assets.Scripts;

namespace Assets.EventSystem.Events
{
    public struct EnemyDiedEvent
    {
        public readonly Enemy Enemy;

        public EnemyDiedEvent(Enemy enemy)
        {
            Enemy = enemy;
        }
    }
}