using System.Collections.Generic;

namespace CLUNL.Graph
{
    public class LogicalGraphNode
    {
        public int ID;
        public LogicalGraphNode ParentGraph;
        public List<LogicalGraphNode> Children;
        public virtual void Visit()
        {

        }
    }
}
