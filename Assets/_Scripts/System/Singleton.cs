using UnityEngine;

namespace Apolos.System
{
    public abstract class Singleton<T> : MonoBehaviour where T : Component
    {
        private static T _instance;
        public static T Instance => _instance;

        private void OnDestroy()
        {
            if (_instance == this)
                _instance = null;
        }

        public virtual void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this as T;
            }
        }
    }
}

