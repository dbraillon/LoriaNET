using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loria.Core
{
    public interface IModule
    {
        string Name { get; }

        bool IsEnabled();
        void Configure();
        void Activate();
        void Deactivate();
    }
}
