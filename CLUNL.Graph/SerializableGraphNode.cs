using System;
using System.Collections.Generic;

namespace CLUNL.Graph
{
    public class SerializableGraphNode
    {
        public int ID;
        public List<int> Children;
        public int Parent;
    }
}
