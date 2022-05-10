using System;
using System.Collections.Generic;

namespace CLUNL.Graph
{
    public class SerializableGraphNode
    {
        public int ID;
        public string OriginalType;
        public Dictionary<int, string> Inputs;
        public Dictionary<string, string> Properties;
        public List<int> Children;
        public int Parent;
    }
}