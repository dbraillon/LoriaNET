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
            var moduleFiles = moduleDir.GetFiles("*.dll").ToList();

            Debug.WriteLine($"Found {moduleFiles.Count} file(s) in '{moduleDirPath}' directory");

            foreach (var moduleFile in moduleFiles)
            {
                var assembly = Assembly.LoadFile(moduleFile.FullName);
                var assemblyModules = assembly.GetTypes()
                    .Where(t =>
                        typeof(IModule).IsAssignableFrom(t) &&
                        t.IsClass && !t.IsAbstract
                    )
                    .Select(t => Activator.CreateInstance(t, engine) as IModule)
                    .ToList();

                Debug.WriteLine($"Loaded '{moduleFile.FullName}'. Found {assemblyModules.Count} module(s).");

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
