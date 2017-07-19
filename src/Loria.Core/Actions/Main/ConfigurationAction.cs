using System;
using System.Linq;
using System.Threading.Tasks;

namespace Loria.Core.Actions.Main
{
    public class ConfigurationAction : IAction
    {
        public string Command => "conf";
        public string Description => "Get or set a property value of configuration object";
        public string Usage => "conf MODIFIER PROPERTY [VALUE]";

        public Configuration Configuration { get; set; }

        public ConfigurationAction(Configuration configuration)
        {
            Configuration = configuration;
        }

        public async Task PerformAsync(string[] args)
        {
            await Task.Delay(0);

            var modifier = args.FirstOrDefault();
            if (modifier != null)
            {
                var propertyName = args.Skip(1).FirstOrDefault();
                var property = Configuration.GetType().GetProperty(propertyName ?? string.Empty);
                if (property != null)
                {
                    if (string.Equals(modifier, "get", StringComparison.CurrentCultureIgnoreCase))
                    {
                        Configuration.TalkBack($"{property.GetValue(Configuration)}");
                    }
                    else if (string.Equals(modifier, "set", StringComparison.CurrentCultureIgnoreCase))
                    {
                        var value = string.Join(" ", args.Skip(2));

                        var constructor = property.PropertyType.GetConstructor(new Type[] { typeof(string) });
                        if (constructor != null)
                        {
                            property.SetValue(Configuration, Activator.CreateInstance(property.PropertyType, value));
                        }
                        else
                        {
                            property.SetValue(Configuration, Convert.ChangeType(value, property.PropertyType));
                        }
                    }
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("Properties:");

                    foreach (var configProperty in Configuration.GetType().GetProperties())
                    {
                        Console.WriteLine($" {configProperty.PropertyType}\t{configProperty.Name}");
                    }

                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("Modifiers:");

                Console.WriteLine($" get\tRetrieve a property value");
                Console.WriteLine($" set\tOverride a property value");

                Console.WriteLine();
            }
        }
    }
}
