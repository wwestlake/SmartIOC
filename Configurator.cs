using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SmartIOC
{
    public enum LifeCycle { Instance, Singleton, PerRequest, PerSession }

　
    public class Configurator
    {
        private static Registry registry = new Registry();

        public LifeCycle LifeCycle { get; set; }
        public Type AbstractType { get; set; }
        public Type ConcreteType { get; set; }

　
        public Configurator Register<Tabstract, Tconcrete>()
        {
            var result = new Configurator { LifeCycle = LifeCycle.Singleton, AbstractType = typeof(Tabstract), ConcreteType = typeof(Tconcrete) };
            return result;
        }

        public Configurator Register(LifeCycle lifecycle, Type Tabstract, Type Tconcrete)
        {
            return new Configurator { LifeCycle = lifecycle, AbstractType = Tabstract, ConcreteType = Tconcrete };
        }

        public static T Resolve<T>() where T: class
        {
            return registry.Resolve<T>();
        }

        public Configurator Instance()
        {
            LifeCycle = SmartIOC.LifeCycle.Instance;
            return this;
        }

        public Configurator Singleton()
        {
            LifeCycle = SmartIOC.LifeCycle.Singleton;
            return this;
        }

        public Configurator PerRequest()
        {
            LifeCycle = SmartIOC.LifeCycle.PerRequest;
            return this;
        }

        public Configurator PerSession()
        {
            LifeCycle = SmartIOC.LifeCycle.PerSession;
            return this;
        }

        public void Build()
        {
            registry.Register(AbstractType, ConcreteType, this);
        }

        private Constructor cache = null;
        private object singleton = null;

　
        internal object Resolve(Type Tconcrete)
        {
            switch (LifeCycle)
            {
                case SmartIOC.LifeCycle.Singleton: return ResolveSingleton(Tconcrete);
                case SmartIOC.LifeCycle.Instance: return ResolveInstance(Tconcrete);
                case SmartIOC.LifeCycle.PerRequest: return ResolvePerRequest(Tconcrete);
                case SmartIOC.LifeCycle.PerSession: return ResolvePerSession(Tconcrete);
                default: return _resolve(Tconcrete);
            }
        }

        private object ResolvePerSession(Type Tconcrete)
        {
            throw new NotImplementedException();
        }

        private object ResolvePerRequest(Type Tconcrete)
        {
            throw new NotImplementedException();
        }

        private object ResolveSingleton(Type Tconcrete)
        {
            return singleton ?? (singleton = _resolve(Tconcrete));
        }

        private object ResolveInstance(Type Tconcrete)
        {
            return _resolve(Tconcrete);
        }

        private object _resolve(Type Tconcrete)
        {
            if (cache == null) cache = new Constructor(Tconcrete);
            
            List<object> args = new List<object>();
            foreach (var param in cache.Parameters)
            {
                args.Add(registry.Resolve(param));
            }

            var result = cache.Instance(args);

            foreach (var field in result.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
            {
                if (Attribute.IsDefined(field, typeof(InjectAttribute)))
                {
                    var value = registry.Resolve( field.FieldType );
                    field.SetValue(result, value);
                }
            }

            return result;
        }
        

    }
}
