using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loria.Tests
{
    public interface IContextBuilder
    {
        Command BuildContext(Command command);
    }
}
