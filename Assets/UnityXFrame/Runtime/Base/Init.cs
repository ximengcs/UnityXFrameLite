using XFrame.Core;
using UnityEngine;
using UnityXFrame.Core.Diagnotics;

namespace UnityXFrame.Core
{
    public class Init : SingletonMono<Init>
    {
        public InitData Data;

        private void Awake()
        {
            Entry.Init();
        }

        private void Start()
        {
            Entry.Start();
        }

        private void Update()
        {
            Entry.Update(Time.deltaTime);
        }

        private void OnGUI()
        {
#if CONSOLE
            Debuger.Inst?.OnGUI();
#endif
        }

        private void OnDestroy()
        {
            Entry.ShutDown();
        }
    }
}