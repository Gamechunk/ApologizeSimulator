using System.Collections.Generic;
using UnityEngine;

namespace ProcessSystem
{
    public class ProcessorLocal : MonoBehaviour
    {
        public IProcess[] Processes { get; private set; }

        private void Awake()
        {
            AwakeMe();
        }

        private void Start()
        {
            StartMe();
        }

        private void Update()
        {
            UpdateMe();
        }



        public void AwakeMe()
        {
            //find all processes
            Processes = GetComponentsInChildren<IProcess>();

            for (int i = 0; i < Processes.Length; i++)
                if (Processes[i].Activated)
                    Processes[i].AwakeMe();
        }

        public void StartMe()
        {
            for (int i = 0; i < Processes.Length; i++)
                if (Processes[i].Activated)
                    Processes[i].StartMe();
        }

        public void UpdateMe()
        {
            for (int i = 0; i < Processes.Length; i++)
                if (Processes[i].Activated)
                    Processes[i].UpdateMe();
        }

        public void DestroyMe()
        {
            for (int i = 0; i < Processes.Length; i++)
                if (Processes[i].Activated)
                    Processes[i].DestroyMe();
        }

        public bool TryGetProcess<T>(out T outProcess) where T : class
        {
            outProcess = null;
            for (int i = 0; i < Processes.Length; i++)
            {
                if (Processes[i] is not T)
                    continue;

                outProcess = Processes[i] as T;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Trying get process
        /// </summary>
        /// <param name="index">If the same process occurs several times, then you should choose an index</param>
        /// <returns></returns>
        public bool TryGetProcess<T>(out T outProcess, int index) where T : class
        {
            var list = new List<T>();

            outProcess = null;
            for (int i = 0; i < Processes.Length; i++)
            {
                if (Processes[i] is not T)
                    continue;

                list.Add(Processes[i] as T);

                if (list.Count - 1 == index)
                {
                    outProcess = Processes[i] as T;
                    return true;
                }
            }

            return false;
        }
    }
}
