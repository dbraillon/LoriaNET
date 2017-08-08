using System;
using System.Collections.Generic;
using System.Linq;

namespace Loria.Core
{
    public class Listeners
    {
        public List<IListener> All { get; }

        public Listeners(List<IListener> listeners)
        {
            All = listeners;
        }

        public IListener Get(string name) => All.FirstOrDefault(l => string.Equals(l.Name, name, StringComparison.InvariantCultureIgnoreCase));

        public void Start(string name) => Get(name)?.Start();
        public void Stop(string name) => Get(name)?.Stop();
        public void Pause(string name) => Get(name)?.Pause();
        public void Resume(string name) => Get(name)?.Resume();

        public void StartAll() => All.ForEach(l => l.Start());
        public void StopAll() => All.ForEach(l => l.Stop());
        public void PauseAll() => All.ForEach(l => l.Pause());
        public void ResumeAll() => All.ForEach(l => l.Resume());
    }
}
