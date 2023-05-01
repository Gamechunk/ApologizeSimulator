using UnityEngine;

namespace ManagersSystem
{
    public abstract class StaticSingleton<T> : MonoBehaviour, IManager where T : MonoBehaviour
    {
        protected StaticSingleton()
        {
            Instance = this as T;
        }

        public static T Instance { get; private set; }
        [field:SerializeField]  public bool IsActivated { get; set; } = true;

        public virtual void AwakeManager() { }
        public virtual void StartManager() { }
        public virtual void UpdateManager() { }
        public virtual void DisposeManager() { }
    }
}
