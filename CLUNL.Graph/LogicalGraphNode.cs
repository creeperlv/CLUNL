using System.Collections.Generic;

namespace CLUNL.Graph
{
    public class LogicalGraphNode
    {
        public int ID;
        public Dictionary<int, LogicalGraphNode> Inputs;
        public LogicalGraphNode ParentGraph;
        public List<LogicalGraphNode> Children;
        public virtual object Execute()
        {
            return null;
        }
    }
}
