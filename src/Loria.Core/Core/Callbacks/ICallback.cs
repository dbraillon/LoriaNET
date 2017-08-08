using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loria.Core
{
    public interface ICallback
    {
        string Name { get; }

        void Callback(string message);
    }
}
