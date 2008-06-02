using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DevDefined.Common.Dsl
{
    public class ComponentNode : AbstractNode
    {
        private Dictionary<string, object> _parameters = new Dictionary<string, object>();
        private Dictionary<string, SectionNode> _sections = new Dictionary<string, SectionNode>();
        
        public string Name { get; set; }

        public Dictionary<string, SectionNode> Sections
        {
            get { return _sections; }
        }
        
        public Dictionary<string, object> Parameters
        {
            get { return _parameters; }
            set { _parameters = value; }
        }
    }
}
