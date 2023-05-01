using ManagersSystem;
using System.Collections;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace ProcessSystem
{
    public enum UpdateType
    {
        EveryOneFrame = 1,
        EveryTwoFrames = 2,
        EveryFourFrames = 4,
    }

    public class ProcessorManager : StaticSingleton<ProcessorManager>
    {
        [SerializeField] private UpdateType UpdateMethod = UpdateType.EveryOneFrame;

        private ProcessorLocal[] _processors;

        private ProcessorLocal[][] _pools;
        int pIter = 0;
        private Coroutine _updatorCoro;


        public override void AwakeManager()
        {
            _processors = FindObjectsOfType<ProcessorLocal>();
            for (int i = 0; i < _processors.Length; i++)
            {
                _processors[i].enabled = false;
                _processors[i].AwakeMe();
            }
        }

        public override void StartManager()
        {
            for (int i = 0; i < _processors.Length; i++)
                _processors[i].StartMe();
            SplitOnPools();
            StartStopUppdator(true);
        }

        private void Update()
        {
            
        }

        public override void DisposeManager()
        {
            for (int i = 0; i < _processors.Length; i++)
                _processors[i].DestroyMe();
            StartStopUppdator(false);
        }

        private void SplitOnPools()
        {
            switch (UpdateMethod)
            {
                case UpdateType.EveryOneFrame:
                    _pools = new ProcessorLocal[1][] { _processors };
                    break;
                case UpdateType.EveryTwoFrames:
                    var center = _processors.Length / 2;
                    _pools = new ProcessorLocal[2][] { 
                        (ProcessorLocal[])_processors.Take(center),
                        (ProcessorLocal[])_processors.Skip(center)};

                    break;
                case UpdateType.EveryFourFrames:
                    var quad = _processors.Length / 4;
                    _pools = new ProcessorLocal[4][] {
                        (ProcessorLocal[])_processors.Take(quad),
                        (ProcessorLocal[])_processors.Skip(quad),
                        (ProcessorLocal[])_processors.Skip(quad * 2),
                        (ProcessorLocal[])_processors.Skip(quad * 3)
                    };
                    break;
                default:
                    break;
            }
        }


        private void StartStopUppdator(bool start)
        {
            if (start)
            {
                if (_updatorCoro != null)
                    return;
                _updatorCoro = StartCoroutine(Updator());
            }
            else
            {
                if (_updatorCoro != null)
                    StopCoroutine(_updatorCoro);
                _updatorCoro = null;
            }
        }

        private IEnumerator Updator()
        {
            do
            {
                if (UpdateMethod == UpdateType.EveryOneFrame)
                {
                    for (int i = 0; i < _processors.Length; i++)
                        _processors[i].UpdateMe();
                    yield return null;
                    continue;
                }
                else
                {

                    for (int i = 0; i < _pools[pIter].Length; i++)
                        _pools[pIter][i].UpdateMe();

                    yield return null;
                    pIter++;

                    if (pIter >= (int)UpdateMethod)
                        pIter = 0;
                }
            } while (true);
        }

        [MenuItem("CustomTools/ProcessSystem/Setup ProcessManager")]
        private static void SetHierarchy()
        {
            var mainGo = FindObjectOfType<ManagersController>();
            if (mainGo == null)
            {
                ManagersController.SetHierarchy();
                mainGo = FindObjectOfType<ManagersController>();
            }

            var procGo = FindObjectOfType<ProcessorManager>();
            if (procGo == null)
            {
                procGo = new GameObject("PROCESS-MANAGER", typeof(ProcessorManager)).GetComponent<ProcessorManager>();
                procGo.transform.SetParent(mainGo.transform);
            }
        }
    }

}