using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ModCore.Utilities.Reflection
{
    public static class ReflectionUtil
    {
        public static T ChangeType<T>(object value)
        {
            var t = typeof(T);

            if (t.IsConstructedGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                {
                    return default(T);
                }

                t = Nullable.GetUnderlyingType(t);
            }

            return (T)Convert.ChangeType(value, t);
        }

        public static object ChangeType(object value, Type conversion)
        {
            var t = conversion;

            if (t.IsConstructedGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                {
                    return null;
                }

                t = Nullable.GetUnderlyingType(t);
            }

            return Convert.ChangeType(value, t);
        }

        public static object ChangeTypeWithEnumConversion(this object value, Type conversion)
        {

            if (conversion.IsConstructedGenericType && conversion.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                {
                    return null;
                }

                conversion = Nullable.GetUnderlyingType(conversion);
            }

            if(conversion.GetTypeInfo().IsEnum)
            {
                value = Convert.ChangeType(value, typeof(System.Int32));
                return Enum.ToObject(conversion, value);
            }

            return Convert.ChangeType(value, conversion);
        }
    }
}
