using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartIOC
{
    [AttributeUsage(AttributeTargets.Field)]
    public class InjectAttribute : Attribute
    {
    }
}
