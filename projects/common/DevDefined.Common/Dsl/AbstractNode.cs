using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DevDefined.Common.Dsl
{
    public abstract class AbstractNode : INode
    {
        private readonly List<INode> _nodes = new List<INode>();

        public INode Parent { get; set; }

        public List<INode> Nodes { get { return _nodes; } }
    }
}
