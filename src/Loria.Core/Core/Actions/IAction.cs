using System.Threading.Tasks;

namespace Loria.Core
{
    public interface IAction
    {
        string Name { get; }
        string Command { get; }
        string Description { get; }
        string Usage { get; }

        void Perform(string[] args);
    }
}
