using System;
using System.Collections.Generic;

namespace Game399.Shared.Runtime
{
    public static class ServiceResolver
    {
        private static readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();

        public static void Register<T>(T service) where T : class
        {
            _services[typeof(T)] = service;
        }

        public static T Resolve<T>() where T : class
        {
            if (_services.TryGetValue(typeof(T), out var service))
            {
                return (T)service;
            }

            throw new InvalidOperationException($"Service of type {typeof(T).Name} not registered");
        }

        public static void Clear()
        {
            _services.Clear();
        }
    }
}