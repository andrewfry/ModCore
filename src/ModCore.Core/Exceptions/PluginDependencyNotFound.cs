using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.Core.Exceptions
{
    public class PluginDependencyNotFound : Exception
    {
        public PluginDependencyNotFound()
            : base() { }

        public PluginDependencyNotFound(string message)
            : base(message) { }

        public PluginDependencyNotFound(string format, params object[] args)
            : base(string.Format(format, args)) { }

        public PluginDependencyNotFound(string message, Exception innerException)
            : base(message, innerException) { }

        public PluginDependencyNotFound(string format, Exception innerException, params object[] args)
            : base(string.Format(format, args), innerException) { }
    }
}
