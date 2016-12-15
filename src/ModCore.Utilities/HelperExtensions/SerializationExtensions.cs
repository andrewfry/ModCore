using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.Utilities.HelperExtensions
{
    public static class SerializationExtensions
    {

        public static string ToJson(this Object obj)
        {
          return  JsonConvert.SerializeObject(obj);
        }

        public static T ToObject<T>(this string jsonString)
        {
            var obj = JsonConvert.DeserializeObject<T>(jsonString);

            return obj;
        }

    }
}
