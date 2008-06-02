using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rhino.Commons;

namespace DevDefined.Common.Dsl
{
    public class ComponentEvaluationScope : IDisposable
    {
        private static string CurrentComponentEvaluationScopeKey = "ComponentEvaluationScopeKey";

        private ComponentEvaluationScope _previous;
        private ComponentNode _componentNode;
        private Dictionary<string, object> _viewParameters = new Dictionary<string, object>();

        public ComponentEvaluationScope(ComponentNode componentNode)
        {
            _componentNode = componentNode;
            _previous = (ComponentEvaluationScope)Local.Data[CurrentComponentEvaluationScopeKey];
            Local.Data[CurrentComponentEvaluationScopeKey] = this;
        }

        public static ComponentEvaluationScope Current
        {
            get
            {
                return (ComponentEvaluationScope)Local.Data[CurrentComponentEvaluationScopeKey];
            }
        }

        public ComponentNode ComponentNode
        {
            get { return _componentNode; }
        }

        public Dictionary<string, object> ViewParameters
        {
            get { return _viewParameters; }
        }

        public void Dispose()
        {
            Local.Data[CurrentComponentEvaluationScopeKey] = _previous;
        }
    }

}
