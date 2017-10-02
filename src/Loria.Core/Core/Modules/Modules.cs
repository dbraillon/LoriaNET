using LoriaNET.Storage;
using System;
using System.Linq;
using System.Reflection;

namespace LoriaNET
{
    /// <summary>
    /// A class that contains a set of modules, allowing user to retrieve a specific 
    /// module or to configure each module in set.
    /// </summary>
    public sealed class Modules
    {
        /// <summary>
        /// A set of actions.
        /// </summary>
        public IModule[] Set { get; set; }

        /// <summary>
        /// Create an instance and load every modules in assembly.
        /// </summary>
        /// <param name="configuration">Loria's configuration.</param>
        public Modules(Loria loria)
        {
            Set = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t =>
                    typeof(IModule).IsAssignableFrom(t) &&
                    t.IsClass && !t.IsAbstract
                )
                .Select(t => Activator.CreateInstance(t, loria) as IModule)
                .ToArray();
        }

        /// <summary>
        /// Retrieve all objects in the set matching the typeparam.
        /// </summary>
        /// <typeparam name="TObject">An object type.</typeparam>
        /// <returns>All objects matching the typeparam.</returns>
        public TObject[] GetAll<TObject>() => Set.Where(c => typeof(TObject).IsAssignableFrom(c.GetType()) && c.IsEnabled()).OfType<TObject>().ToArray();

        /// <summary>
        /// Configure all module in the set.
        /// </summary>
        public void ConfigureAll()
        {
            foreach (var module in Set)
            {
                try
                {
                    module.Configure();
                }
                catch
                {
                    // TODO
                }
            }
        }
    }
}
