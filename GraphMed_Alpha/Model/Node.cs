using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GraphMed_Alpha.Model
{
    public abstract class Node
    {
        public PropertyInfo GetField(string propertyName)
        {
            return this.GetType().GetProperty(propertyName.First().ToString().ToUpper() + propertyName.Substring(1));
        }
    }
}
