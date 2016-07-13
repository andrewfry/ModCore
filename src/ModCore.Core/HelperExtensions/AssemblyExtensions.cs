﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;

namespace ModCore.Core.HelperExtensions
{
    public static class AssemblyExtensions
    {
        public static Type GetImplementation<T>(this IEnumerable<Assembly> assemblies)
        {
            return assemblies.GetImplementation<T>(null);
        }

        public static Type GetImplementation<T>(this IEnumerable<Assembly> assemblies, Func<Assembly, bool> predicate)
        {
            IEnumerable<Type> implementations = assemblies.GetImplementations<T>(predicate);

            if (implementations.Count() == 0)
                throw new ArgumentException("Implementation of " + typeof(T) + " not found");

            return implementations.FirstOrDefault();
        }

        public static IEnumerable<Type> GetImplementations<T>(this IEnumerable<Assembly> assemblies)
        {
            return assemblies.GetImplementations<T>(null);
        }

        public static IEnumerable<Type> GetImplementations<T>(this IEnumerable<Assembly> assemblies, Func<Assembly, bool> predicate)
        {
            List<Type> implementations = new List<Type>();

            foreach (Assembly assembly in assemblies)
                foreach (Type type in assembly.GetTypes())
                    if (typeof(T).GetTypeInfo().IsAssignableFrom(type) && type.GetTypeInfo().IsClass)
                        implementations.Add(type);

            return implementations;
        }

        public static Type GetImplementationOrDefault<T>(this Assembly assembly)
        {

            foreach (Type type in assembly.GetTypes())
                if (typeof(T).GetTypeInfo().IsAssignableFrom(type) && type.GetTypeInfo().IsClass)
                {
                    return type;
                }

            return null;
        }

        public static T GetInstance<T>(this IEnumerable<Assembly> assemblies)
        {
            return assemblies.GetInstance<T>(null);
        }

        public static T GetInstance<T>(this IEnumerable<Assembly> assemblies, Func<Assembly, bool> predicate)
        {
            IEnumerable<T> instances = assemblies.GetInstances<T>(predicate);

            if (instances.Count() == 0)
                throw new ArgumentException("Instance of " + typeof(T) + " can't be created");

            return instances.FirstOrDefault();
        }

        public static IEnumerable<T> GetInstances<T>(this IEnumerable<Assembly> assemblies)
        {
            return assemblies.GetInstances<T>(null);
        }

        public static IEnumerable<T> GetInstances<T>(this IEnumerable<Assembly> assemblies, Func<Assembly, bool> predicate)
        {
            List<T> instances = new List<T>();

            foreach (Type implementation in assemblies.GetImplementations<T>())
            {
                T instance = (T)Activator.CreateInstance(implementation);

                instances.Add(instance);
            }

            return instances;
        }

        public static T GetInstance<T>(this Assembly assembly)
        {
            Type implementation = assembly.GetImplementationOrDefault<T>();
            return  (T)Activator.CreateInstance(implementation);
        }
    }
}
