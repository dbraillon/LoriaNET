﻿using System;
using System.Linq;

namespace LoriaNET
{
    /// <summary>
    /// Loria's hub object, used to propagate commands or callbacks to every modules.
    /// </summary>
    internal sealed class Hub
    {
        /// <summary>
        /// Loria's configuration.
        /// </summary>
        internal Configuration Configuration { get; }

        /// <summary>
        /// Create a new instance of Loria's hub.
        /// </summary>
        /// <param name="configuration">Loria's configuration.</param>
        internal Hub(Configuration configuration)
        {
            Configuration = configuration;
        }
        
        /// <summary>
        /// Propagate a command or a callback, make sure you prefix the phrase with callback keyword.
        /// </summary>
        /// <param name="commandOrCallback">
        ///     By default a command, unless the phrase is prefixed 
        ///     with callback keyword.
        /// </param>
        internal void Propagate(string commandOrCallback)
        {
            if (string.IsNullOrEmpty(commandOrCallback)) return;

            // Split the command or callback to lookup for callback keyword
            var commandOrCallbackSplitted = commandOrCallback.Split(' ');
            var direction = commandOrCallbackSplitted.ElementAtOrDefault(0);

            // Check if callback keyword is here
            if (string.Equals(direction, Callbacks.Keyword, StringComparison.InvariantCultureIgnoreCase))
            {
                PropagateCallback(string.Join(" ", commandOrCallbackSplitted.Skip(1)));
            }
            else
            {
                PropagateCommand(new Command(commandOrCallback));
            }
        }

        /// <summary>
        /// Propagate a command to corresponding enabled action.
        /// </summary>
        /// <param name="command">A command.</param>
        internal void PropagateCommand(Command command)
        {
            if (command == null) return;
            
            // Try to find corresponding action
            var action = Configuration.Actions.GetByCommand(command.Module);
            if (action != null)
            {
                // If an action match the command, perform it
                action.Perform(command);
            }
            else
            {
                // If not, callback a command not found message
                Configuration.Callbacks.Propagate(Resources.Strings.CommandNotFound);
            }
        }

        /// <summary>
        /// Propagate a callback to all enabled callbacks.
        /// </summary>
        /// <param name="message">A message to callback.</param>
        internal void PropagateCallback(string message)
        {
            if (string.IsNullOrEmpty(message)) return;

            // Propagate to all enabled callbacks
            Configuration.Callbacks.Propagate(message);
        }
    }
}