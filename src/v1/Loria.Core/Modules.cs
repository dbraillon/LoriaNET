using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Loria.Core
{
    public class Modules : Set<IModule>
    {
        public Modules(Engine engine)
        {
            var modules = new List<IModule>();
            var moduleDirPath = Path.Combine(Directory.GetCurrentDirectory(), "modules");
            var moduleDir = Directory.CreateDirectory(moduleDirPath);
            var dlls = moduleDir.GetFiles("*.dll", SearchOption.AllDirectories)
                .Select(dll => Assembly.LoadFile(dll.FullName));

            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler((sender, args) =>
            {
                var assemblyName = new AssemblyName(args.Name);
                var assembly = dlls.FirstOrDefault(x => x.GetName().Name == assemblyName.Name);
                return assembly;
            });

            foreach (var dll in dlls)
            {
                var assemblyModules = dll.GetTypes()
                    .Where(t =>
                        typeof(IModule).IsAssignableFrom(t) &&
                        t.IsClass && !t.IsAbstract
                    )
                    .Select(t => Activator.CreateInstance(t, engine) as IModule)
                    .ToList();

                Debug.WriteLine($"Loaded '{dll.FullName}'. Found {assemblyModules.Count} module(s).");

                foreach (var module in assemblyModules)
                {
                    Debug.WriteLine($"Loaded '{module.Name}'");
                    modules.Add(module);
                }
            }

            Objects = modules.ToArray();
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
                    Debug.WriteLine($"{module.Name} successfuly configured");
                }
                catch
                {
                    Debug.WriteLine($"{module.Name} fails to configure");
                }
            }
        }
    }
}
