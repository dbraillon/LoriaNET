namespace LoriaNET
{
    /// <summary>
    /// A class providing a simple way to implement the module interface.
    /// </summary>
    internal abstract class Module : IModule
    {
        public Configuration Configuration { get; set; }
        public bool Enabled { get; set; }

        public abstract string Name { get; }

        public Module(Configuration configuration)
        {
            Configuration = configuration;
        }

        public void Activate() => Enabled = true;
        public void Deactivate() => Enabled = false;
        public bool IsEnabled() => Enabled;

        public abstract void Configure();
    }
}
