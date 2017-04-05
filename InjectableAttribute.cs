using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartIOC
{
    [AttributeUsage(AttributeTargets.Class)]
    public class InjectableAttribute : Attribute
    {
        public Type AbstractType { get; set; }
        public LifeCycle LifeCycle { get; set; }

        public InjectableAttribute(Type abstractType, LifeCycle lifeCycle)
        {
            this.AbstractType = abstractType;
            this.LifeCycle = lifeCycle;
        }
    }
}
