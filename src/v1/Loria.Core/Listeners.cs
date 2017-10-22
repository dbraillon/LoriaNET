using System;
using System.Collections.Generic;

namespace Loria.Core
{
    public class Listeners : Set<IListener>
    {
        public Listeners(params IListener[] objects) : base(objects)
        {
        }
        public Listeners(IEnumerable<IListener> objects) : base(objects)
        {
        }
        
        public void StartAll()
        {
            foreach (var listener in Objects)
            {
                listener.Start();
            }
        }
        
        public void StopAll()
        {
            foreach (var listener in Objects)
            {
                listener.Stop();
            }
        }
        
        public void PauseAll()
        {
            foreach (var listener in Objects)
            {
                listener.Pause();
            }
        }
        
        public void ResumeAll()
        {
            foreach (var listener in Objects)
            {
                listener.Resume();
            }
        }
    }
}
