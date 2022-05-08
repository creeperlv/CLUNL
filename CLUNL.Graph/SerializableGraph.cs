using System.Collections.Generic;

namespace CLUNL.Graph
{
    public class SerializableGraph
    {
        public List<SerializableGraphNode> Nodes;
        public LogicalGraph Deserialize()
        {
            LogicalGraph graph = new LogicalGraph();
            return graph;
        }
    }
}
