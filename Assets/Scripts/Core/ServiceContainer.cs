using System;
using System.Collections.Generic;

namespace Core
{
    public class ServiceContainer
    {
        private readonly Dictionary<Type, object> services = new();

        public void Register<T>(T instance)
        {
            services.Add(typeof(T), instance);
        }

        public T Resolve<T>()
        {
            if (services.TryGetValue(typeof(T), out var service))
            {
                return (T)service;
            }
            else
            {
                throw new Exception($"Service {typeof(T)} not found in service container.");
            }
        }
    }
}