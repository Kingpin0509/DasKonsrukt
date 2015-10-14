using Assets.Scripts.V2;

namespace Assets.EventSystem.Events
{
    public class TowerOrPadWasPressedEvent
    {
        public readonly LinkableSceneItem SceneItem;

        public TowerOrPadWasPressedEvent(LinkableSceneItem sceneItem)
        {
            SceneItem = sceneItem;
        }
    }
}