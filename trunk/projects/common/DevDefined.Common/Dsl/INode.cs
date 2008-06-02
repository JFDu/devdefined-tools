using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DevDefined.Common.Dsl
{
    public interface INode
    {
        INode Parent { get; set; }
        List<INode> Nodes { get; }
    }
}
