using System;
using System.Collections.Generic;
using UserGuiLib.Common.Component;
using UserGuiLib.Common.Drawing;
using UserGuiLib.Common.Services;

namespace UserGuiLib.Common.Impl
{
    public class Component : IComponent
    {
        public ITransform Transform { get; private set; }

        public Component()
        {
            Transform = new Transform(this, null);
        }

        private Dictionary<Type, object> services = new Dictionary<Type, object>();
        public T GetService<T>() where T : IService
        {
            if (services.ContainsKey(typeof(T)))
                return (T)services[typeof(T)];

            return default(T);
        }

        public T RegisterService<T>(T service) where T : IService
        {
            service.Owner = this;
            services[typeof(T)] = service;
            return service;
        }

        public static AnyFont DefaultFont = new AnyFont("MS Sans Serif", 10);
        public static AnyPen DefaultPen = new AnyPen(255, 0, 0, 0, 1);
    }
}
