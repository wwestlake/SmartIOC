using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SmartIOC
{
    public class Scanner
    {
        public void Scan(Assembly toScan, Configurator config)
        {
            foreach (var type in toScan.GetTypes())
            {
                InjectableAttribute attrib = (InjectableAttribute)Attribute.GetCustomAttribute(type, typeof(InjectableAttribute));
                if (attrib != null)
                {
                    config.Register(attrib.LifeCycle, attrib.AbstractType, type).Build();
                }
            }
        }

        public void Scan(Assembly assembly)
        {
            Scan(assembly, new Configurator());
        }

        private static Scanner _instance;

        public static Scanner Instance
        {
            get
            {
                return _instance ?? (_instance = new Scanner());
            }
        }

    }
}
