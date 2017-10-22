using System;
using System.Collections.Generic;
using System.Linq;

namespace Loria.Core
{
    public class ActionCommand
    {
        public string Raw { get; set; }
        
        public string Intent { get; set; }
        public Entity[] Entities { get; set; }
        
        public ActionCommand(string str)
        {
            // First, retrieve intent
            var separator = ' ';
            var commandSplitted = str.Split(separator);
            Intent = commandSplitted.ElementAtOrDefault(0);

            // Then, retrieve entities
            var entitiesStr = commandSplitted.Skip(1);
            var entities = new List<Entity>();

            foreach (var entityStr in entitiesStr)
            {
                // If a part start with '-' symbol then its the name part
                // if not, its still the value part

                if (entityStr.StartsWith("-"))
                {
                    entities.Add(new Entity(entityStr.Substring(1), string.Empty));
                }
                else
                {
                    var lastEntity = entities.LastOrDefault();
                    if (lastEntity != null)
                    {
                        lastEntity.Value = $"{lastEntity.Value}{separator}{entityStr}".Trim();
                    }
                }
            }

            Entities = entities.ToArray();
            Raw = str;
        }

        public Entity GetEntity(string name)
        {
            return Entities.FirstOrDefault(e => string.Equals(e.Name, name, StringComparison.InvariantCultureIgnoreCase));
        }

        public override string ToString() => Raw;
    }
}
