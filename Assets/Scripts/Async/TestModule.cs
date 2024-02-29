using System.Collections.Generic;
using XFrame.Core;

namespace Async
{
    [CommonModule]
    public class TestModule : SingletonModule<TestModule>, IUpdater
    {
        private HashSet<IUpdater> m_Updaters;

        protected override void OnInit(object data)
        {
            base.OnInit(data);
            m_Updaters = new HashSet<IUpdater>();
        }

        public void Register(IUpdater updaterTask)
        {
            m_Updaters.Add(updaterTask);
        }

        public void UnRegister(IUpdater updaterTask)
        {
            m_Updaters.Remove(updaterTask);
        }
        
        public void OnUpdate(float escapeTime)
        {
            List<IUpdater> updaters = new List<IUpdater>(m_Updaters);
            foreach (IUpdater updater in updaters)
            {
                updater.OnUpdate(escapeTime);
            }
            
        }
    }
}