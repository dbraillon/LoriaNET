using LoriaNET.Storage;

namespace LoriaNET
{
    /// <summary>
    /// A class providing a simple way to implement the module interface.
    /// </summary>
    public abstract class Module : IModule
    {
        public Loria Loria { get; set; }
        public bool Enabled { get; set; }

        public abstract string Name { get; }

        public Module(Loria loria)
        {
            Loria = loria;
        }

        public void Activate() => Enabled = true;
        public void Deactivate() => Enabled = false;
        public bool IsEnabled() => Enabled;

        public abstract void Configure();
    }
}
