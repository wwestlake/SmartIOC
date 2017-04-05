using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace SmartIOC
{
    public class Registry
    {

        private Dictionary<string, RegistryEntry> registry = new Dictionary<string, RegistryEntry>();

        internal void Register(Type abstractType, Type concreteType, Configurator config)
        {
            var hash = HashCode(abstractType);
            if (registry.ContainsKey(hash)) throw new AlreadyRegisteredException();
            registry.Add(hash, new RegistryEntry { Config = config, AbstractType = abstractType, ConcreteType = concreteType });
        }

        internal TAbstract Resolve<TAbstract>() where TAbstract: class
        {
            return Resolve(typeof(TAbstract)) as TAbstract;
        }

        internal object Resolve(Type Tabstract)
        {
            var hash = HashCode(Tabstract);
            if (!registry.ContainsKey(hash)) throw new NotRegisteredException();
            var entry = registry[hash];
            return entry.Config.Resolve(entry.ConcreteType);
        }

        internal class RegistryEntry
        {
            public Configurator Config { get; set; }
            public Type AbstractType { get; set; }
            public Type ConcreteType { get; set; }
        }

        private string HashCode(Type type)
        {
            byte[] toBytes = Encoding.ASCII.GetBytes(type.FullName);
            SHA1 sha = new SHA1CryptoServiceProvider();
            byte[] result = sha.ComputeHash(toBytes);
            return Encoding.ASCII.GetString(result);
        }

　
　
    }
}
