using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.Models.Core
{
    public class ResultPacket<T>
    {
        public bool Successful { get; set; }

        public List<string> ErrorMessages { get; set; }

        public Exception Exception { get; set; }

        public T Result { get; set; }
    }

    public class SuccessPacket<T> : ResultPacket<T>
    {
        public SuccessPacket(T result)
        {
            this.Result = result;
            this.Successful = true; 
        } 
    }

    public class FailurePacket<T> : ResultPacket<T>
    {
        public FailurePacket(Exception ex, params string[] customErrorMessages)
        {
            this.Successful = false;
            this.Exception = ex;
            this.ErrorMessages = customErrorMessages.ToList();
        }
    }
}
