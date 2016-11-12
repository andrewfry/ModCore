using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.Services.MongoDb.Exceptions
{
    public class DuplicateUserException : Exception
    {
        public DuplicateUserException()
        {
        }

        public DuplicateUserException(string message)
            : base(message)
        {
        }

        public DuplicateUserException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
