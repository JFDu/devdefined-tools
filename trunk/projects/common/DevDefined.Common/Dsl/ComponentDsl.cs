using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevDefined.Common.Hash;

namespace DevDefined.Common.Dsl
{
    public class ComponentDsl : StandardDsl
    {
        public ComponentDsl()
        {
        }

        public Batch[] Component(params Batch[] batches)
        {
            Batch asBatch = new Batch(delegate(Batch ignore)
            {
                ComponentNode componentNode = new ComponentNode()
                  {
                      Name = NodeWriter.ExtractName
                  };

                NodeWriter.WriteStartNode(componentNode);
                ExecuteBatches(batches);
                NodeWriter.WriteEndNode();

                return null;
            });

            asBatch.Ignore();

            return new Batch[] { asBatch };
        }

        public Batch Parameters(params Func<string, object>[] hash)
        {
            return Parameters(Hasher.Hash<object>(hash));
        }

        public Batch Parameters(Dictionary<string, object> parameters)
        {
            return new Batch(delegate
            {
                FindNode<ComponentNode>().Parameters = parameters;

                return null;
            });
        }

        public Batch[] Section(params Batch[] batches)
        {
            return new Batch[] {delegate
            {
                string name = NodeWriter.ExtractName;
                
                SectionNode sectionNode = new SectionNode();
                FindNode<ComponentNode>().Sections.Add(name, sectionNode);

                NodeWriter nodeWriter = new NodeWriter();
                
                using (new DslEvaluationScope(nodeWriter))
                {
                    ExecuteBatches(batches);
                }
                
                sectionNode.Nodes.AddRange(nodeWriter.Nodes);

                return null;
            }};
        }

        public Batch[] Item<T>(Func<T, string> func)
        {
            return new Batch[] { delegate
            {
                NodeWriter.WriteNode(new ItemNode<T>(func));
                return null;
            }};
        }

        private T FindNode<T>() where T : class, INode
        {
            INode currentNode = NodeWriter.CurrentNode;

            while (currentNode != null)
            {
                var typedNode = currentNode as T;
                if (typedNode != null) return typedNode;
                currentNode = currentNode.Parent;
            }

            return null;
        }
    }
}
