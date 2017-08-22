using System;
using System.Linq;

namespace LoriaNET
{
    /// <summary>
    /// A class that contains a set of callbacks, allowing user to retrieve a specific 
    /// callback or to propagate a message to every callbacks in set.
    /// </summary>
    internal sealed class Callbacks
    {
        /// <summary>
        /// Keyword used to make the difference when calling a callback or an action.
        /// </summary>
        internal const string Keyword = "callback";

        /// <summary>
        /// A set of callbacks.
        /// </summary>
        private ICallback[] Set { get; }

        /// <summary>
        /// Create an instance with a pre defined set of callbacks.
        /// </summary>
        /// <param name="callbacks">A set of callbacks.</param>
        internal Callbacks(params ICallback[] callbacks)
        {
            Set = callbacks;
        }

        /// <summary>
        /// Retrieve a callback by its name.
        /// </summary>
        /// <param name="name">A name of a callback.</param>
        /// <returns>Found callback or null.</returns>
        internal ICallback Get(string name) => Set.FirstOrDefault(l => string.Equals(l.Name, name, StringComparison.InvariantCultureIgnoreCase));

        /// <summary>
        /// Propagate a message to every callback in the set.
        /// </summary>
        /// <param name="message">A message to propagate.</param>
        internal void Propagate(string message)
        {
            foreach (var callback in Set)
            {
                callback.Callback(message);
            }
        }
    }
}
