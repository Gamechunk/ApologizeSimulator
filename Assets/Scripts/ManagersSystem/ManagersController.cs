using System.Linq;
using UnityEditor;
using UnityEngine;

namespace ManagersSystem
{
    public class ManagersController : MonoBehaviour
    {
        private IManager[] _managers;
        private IExecuteAfterManagers[] _executedAfterManagers;

        private void Awake()
        {
            _managers = GetComponentsInChildren<IManager>();
            GameObject[] objs = FindObjectsOfType<GameObject>().Where((m) => m.TryGetComponent<IExecuteAfterManagers>(out var c)).ToArray();

            _executedAfterManagers = new IExecuteAfterManagers[objs.Length];
            for (int j = 0; j < objs.Length; j++)
                _executedAfterManagers[j] = objs[j].GetComponent<IExecuteAfterManagers>();


            for (int i = 0; i < _managers.Length; i++)
                _managers[i].AwakeManager();
        }

        private void Start()
        {
            for (int i = 0; i < _managers.Length; i++)
            {
                _managers[i].StartManager();
            }

            if (_executedAfterManagers != null)
            {
                for (int i = 0; i < _executedAfterManagers.Length; i++)
                    _executedAfterManagers[i].ExecuteAwake();
            }

            if (_executedAfterManagers != null)
            {
                for (int i = 0; i < _executedAfterManagers.Length; i++)
                    _executedAfterManagers[i].ExecuteStart();
            }
        }

        private void OnDestroy()
        {
            for (int i = 0; i < _managers.Length; i++)
            {
                _managers[i].DisposeManager();
            }
        }

        private void Update()
        {
            for (int i = 0; i < _managers.Length; i++)
            {
                _managers[i].UpdateManager();
            }
        }

        [MenuItem("CustomTools/Setup Managers Layout")]
        public static void SetHierarchy()
        {
            if (FindObjectOfType<ManagersController>())
                return;

            var mainGo = new GameObject("[MANAGERS]", typeof(ManagersController));
            var manager1 = new GameObject("Manager1");
            manager1.transform.SetParent(mainGo.transform);
        }
    }

}