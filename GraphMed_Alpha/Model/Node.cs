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
        public string LinkProp { get; protected set; }
        
        public Node() { }

        public Node(string linkProp)
        {
            this.LinkProp = linkProp; 
        }

        public PropertyInfo GetField(string propertyName)
        {
            return this.GetType().GetProperty(propertyName.First().ToString().ToUpper() + propertyName.Substring(1));
        }
    }
}
