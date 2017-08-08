using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Loria.Core
{
    public class ActionFactory
    {
        public static IEnumerable<IAction> GetAllActions(Configuration configuration)
        {
            return Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => 
                    t.GetInterfaces().Contains(typeof(IAction)) && 
                    t.IsClass
                )
                .Select(t => Activator.CreateInstance(t, configuration) as IAction);
        }
    }
}
