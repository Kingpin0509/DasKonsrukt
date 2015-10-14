using Assets.Scripts.Towers;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// Helper class used to simplify the Transform and GameObject reference caching
    /// </summary>
    public class BaseGameObject : MonoBehaviour
    {
        public Transform TransformCache { get; set; }
        public GameObject GameObjectCache { get; set; }

        protected virtual void Awake()
        {
            TransformCache = GetComponent<Transform>();
            GameObjectCache = gameObject;
        }

        public virtual void Destroy()
        {
            Destroy(gameObject);
        }
    }
}