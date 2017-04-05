using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SmartIOC
{
    public class Constructor
    {
        Type type;
        ConstructorInfo info;
        string typeName;

        public Constructor(Type type)
        {
            this.type = type;
            typeName = type.FullName;
            info = (from c in type.GetConstructors().OrderByDescending(x => x.GetParameters().Length)
                               where
                                   (from p in c.GetParameters() select p).All(a => a.ParameterType.IsInterface || a.ParameterType.IsAbstract)
                               select c).FirstOrDefault();
        }

        public IEnumerable<Type> Parameters
        {
            get
            {
                foreach (var param in info.GetParameters())
                {
                    yield return param.ParameterType;
                }
            }
        }

        public object Instance(List<object> args)
        {
            var argsArray = args.ToArray();
            return Activator.CreateInstance(type, argsArray);
        }

　
    }
}
