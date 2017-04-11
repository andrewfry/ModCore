﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ModCore.Models.PluginApi
{
    public class ApiResponse
    {
        public object Value { get; set; }
        public bool Success { get; set; }
        public List<string> HandledBy { get; set; }
        public Exception Exception { get; set; }


        public T GetValue<T>()
        {
            if(Value is T)
            {
                return (T)(Value);
            }

            return default(T);
        }
    }
}
