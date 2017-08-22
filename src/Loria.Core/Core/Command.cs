using System;
using System.Collections.Generic;
using System.Linq;

namespace LoriaNET
{
    /// <summary>
    /// A class to parse Loria's command.
    /// </summary>
    public class Command
    {
        // Example of valid command:
        //      spotify play -artist Linkin Park
        //      reminder set -date 20170821104100 -text Call the doctor
        
        /// <summary>
        /// Module part, example: reminder.
        /// </summary>
        public string Module { get; set; }

        /// <summary>
        /// Intent part, example: set.
        /// </summary>
        public string Intent { get; set; }

        /// <summary>
        /// Entities part, example: -date 20170821113300.
        /// </summary>
        public Entity[] Entities { get; set; }

        /// <summary>
        /// Create an instance of a command by parsing a string.
        /// </summary>
        /// <param name="commandStr">A string to parse.</param>
        public Command(string commandStr)
        {
            if (string.IsNullOrEmpty(commandStr)) return;

            // First, retrieve module and intent
            var commandSplitted = commandStr.Split(' ');
            Module = commandSplitted.ElementAtOrDefault(0);
            Intent = commandSplitted.ElementAtOrDefault(1);

            // Then, retrieve entities
            var entitiesStr = commandSplitted.Skip(2);
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
                        lastEntity.Value = $"{lastEntity.Value} {entityStr}".Trim();
                    }
                }
            }

            Entities = entities.ToArray();
        }

        /// <summary>
        /// Retrieve an entity by its name.
        /// </summary>
        /// <param name="name">An entity name.</param>
        /// <returns>Found entity or null.</returns>
        public Entity GetEntity(string name) => Entities.FirstOrDefault(e => string.Equals(e.Name, name, StringComparison.InvariantCultureIgnoreCase));

        public override string ToString() => $"{Module} {Intent} {string.Join(" ", Entities.Select(e => e.ToString()))}";
    }
}
