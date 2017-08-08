using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Loria.Core
{
    public class Loria
    {
        public bool IsLiving { get; protected set; }

        public Configuration Configuration { get; set; }
        
        public Loria(Configuration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Start Loria and block the current thread.
        /// </summary>
        public void Live()
        {
            LiveAsync();

            while (IsLiving)
            {
                Thread.Sleep(1000);
            }
        }

        /// <summary>
        /// Stop Loria smoothly.
        /// </summary>
        public void Stop()
        {
            // Stop listeners
            Configuration.Listeners.StopAll();

            // Everything have been stopped
            IsLiving = false;
        }

        /// <summary>
        /// Start Loria and return as soon as everything has been set up.
        /// </summary>
        public void LiveAsync()
        {
            IsLiving = true;

            // Start enabled listeners
            Configuration.Listeners.StartAll();
        }
    }
}
