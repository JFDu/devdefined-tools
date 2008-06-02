using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rhino.Commons;

namespace DevDefined.Common.Dsl
{
    public class DslEvaluationScope : IDisposable
    {
        private static string CurrentDslEvaluationScopeKey = "CurrentDslEvaluationScopeKey";

        private DslEvaluationScope _previous;
        private NodeWriter _nodeWriter;

        public DslEvaluationScope(NodeWriter nodeWriter)
        {
            _nodeWriter = nodeWriter;
            _previous = (DslEvaluationScope)Local.Data[CurrentDslEvaluationScopeKey];
            Local.Data[CurrentDslEvaluationScopeKey] = this;
        }

        public static DslEvaluationScope Current
        {
            get
            {
                return (DslEvaluationScope)Local.Data[CurrentDslEvaluationScopeKey];
            }
        }

        public NodeWriter NodeWriter
        {
            get { return _nodeWriter; }
        }

        public void Dispose()
        {
            Local.Data[CurrentDslEvaluationScopeKey] = _previous;
        }
    }

}
