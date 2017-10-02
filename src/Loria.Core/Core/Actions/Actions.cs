﻿using System;
using System.Linq;

namespace LoriaNET
{
    /// <summary>
    /// A class that contains a set of actions, allowing user to retrieve a specific 
    /// action or to propagate a command to every actions in set.
    /// </summary>
    public sealed class Actions
    {
        /// <summary>
        /// Keyword used to make the difference when calling a callback or an action.
        /// </summary>
        public const string Keyword = "perform";

        /// <summary>
        /// A set of actions.
        /// </summary>
        public IAction[] Set { get; }

        /// <summary>
        /// Create an instance with a pre defined set of actions.
        /// </summary>
        /// <param name="actions">A set of actions.</param>
        public Actions(params IAction[] actions)
        {
            Set = actions;
        }

        /// <summary>
        /// Retrieve an action by its name.
        /// </summary>
        /// <param name="name">A name of an action.</param>
        /// <returns>Found action or null.</returns>
        public IAction Get(string name) => Set.FirstOrDefault(l => string.Equals(l.Name, name, StringComparison.InvariantCultureIgnoreCase));

        /// <summary>
        /// Retrieve an action by its command.
        /// </summary>
        /// <param name="command">A command of an action.</param>
        /// <returns>Found action or null.</returns>
        public IAction GetByCommand(string command) => Set.FirstOrDefault(l => string.Equals(l.Command, command, StringComparison.InvariantCultureIgnoreCase));
    }
}
