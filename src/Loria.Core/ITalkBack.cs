using System.Threading.Tasks;

namespace Loria.Core
{
    public interface ITalkBack
    {
        Task TalkBackAsync(string message);
    }
}
