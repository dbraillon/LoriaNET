using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Loria.Core
{
    public class Modules
    {
        public List<IModule> All { get; set; }

        public Modules(Configuration configuration)
        {
            All = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t =>
                    t.GetInterfaces().Contains(typeof(IModule)) &&
                    t.IsClass && !t.IsAbstract
                )
                .Select(t => Activator.CreateInstance(t, configuration) as IModule)
                .ToList();
        }

        public void ConfigureAll() => All.ForEach(m => m.Configure());
    }
}
