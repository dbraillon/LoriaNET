using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace LoriaNET
{
    public class Modules : Set<IModule>
    {   
        public Modules()
        {
            Objects = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t =>
                    typeof(IModule).IsAssignableFrom(t) &&
                    t.IsClass && !t.IsAbstract
                )
                .Select(t => Activator.CreateInstance(t) as IModule)
                .ToArray();
        }

        public IEnumerable<TObject> GetAll<TObject>()
        {
            return Objects
                .Where(c => 
                    typeof(TObject).IsAssignableFrom(c.GetType()) && 
                    c.IsEnabled()
                )
                .OfType<TObject>();
        }
        
        public void ConfigureAll()
        {
            foreach (var module in Objects)
            {
                try
                {
                    module.Configure();
                }
                catch
                {
                }
            }
        }
    }
}
