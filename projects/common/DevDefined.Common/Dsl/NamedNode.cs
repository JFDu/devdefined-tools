using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DevDefined.Common.Dsl
{
    public class NamedNode : AbstractNode
    {
        public NamedNode()
        {
        }

        public NamedNode(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}
