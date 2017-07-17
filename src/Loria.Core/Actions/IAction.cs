using System.Threading.Tasks;

namespace Loria.Core.Actions
{
    public interface IAction
    {
        string Command { get; }
        string Description { get; }
        string Usage { get; }
        Task PerformAsync(string[] args);
    }
}
