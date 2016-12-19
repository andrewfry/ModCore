using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore
{
    public static class GlobalExtensions
    {
        public static void ThrowIfNullArgument(this object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException();
            }
        }

        public static void ThrowIfNullArgument(this object obj, string paraName)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(paraName);
            }
        }
    }
}