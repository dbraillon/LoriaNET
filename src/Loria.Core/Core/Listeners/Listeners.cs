using System;
using System.Collections.Generic;

namespace LoriaNET
{
    public class Listeners : Set<IListener>
    {
        public event EventHandler<ListenerEventArgs> OnEvent;

        public Listeners(params IListener[] objects) : base(objects)
        {
        }
        public Listeners(IEnumerable<IListener> objects) : base(objects)
        {
            foreach (var obj in objects)
            {
                obj.OnEvent += EventPropagator;
            }
        }

        private void EventPropagator(object sender, ListenerEventArgs e)
        {
            OnEvent?.Invoke(sender, e);
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
