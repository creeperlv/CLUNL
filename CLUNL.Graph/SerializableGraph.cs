using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace CLUNL.Graph
{
    public class SerializableGraph
    {
        public List<SerializableGraphNode> Nodes;
        public static List<string> AssemblyFilterPrefixes = new List<string>() { "System.", "Microsoft.", "Newtonsoft", "NuGet." };
        public static List<string> AssemblyFilterBlocks = new List<string>() { "netstandard", "testhost", "CLUNL" };
        public static Predicate<Assembly> AssemblyFilter = (item) =>
        {
            var n = item.GetName().Name;
            return StartsWithAny(n, AssemblyFilterPrefixes) ||
        EqualsAny(n, AssemblyFilterBlocks);
        };
        public LogicalGraph Deserialize()
        {
            LogicalGraph graph = new LogicalGraph();
            List<LogicalGraph> children = new List<LogicalGraph>();
            foreach (var node in Nodes)
            {
                var t = node.OriginalType;

            }
            return graph;
        }
        static List<Assembly> Asms = null;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool StartsWithAny(string str, List<string> prefixes)
        {
            foreach (var item in prefixes)
            {
                if (str.StartsWith(item)) return true;
            }
            return false;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool EqualsAny(string str, List<string> strs)
        {
            foreach (var item in strs)
            {
                if (str == item) return true;
            }
            return false;
        }
        public static void CollectAssemblies()
        {
            Asms = AppDomain.CurrentDomain.GetAssemblies().ToList();
            Asms.RemoveAll(AssemblyFilter);
            //for (int i = Asms.Count-1; i >= 0; i--)
            //{
            //    var item = Asms[i];
            //    if (item.FullName.StartsWith("System.")|| item.FullName.StartsWith("Microsoft."))
            //    {
            //        Asms.RemoveAt(i);
            //    }
            //}
#if TRACE
            Trace.WriteLine($"Collected {Asms.Count} assemblies in total.");
            foreach (var item in Asms)
            {
                Trace.WriteLine(item.FullName);
            }
#endif
        }
        public static Type FindType(string TypeName)
        {
            if (Asms == null) CollectAssemblies();
            foreach (var item in Asms)
            {
                var t = item.GetType(TypeName);
                if (t != null)
                {
                    return t;
                }
            }
            return null;
        }
    }
}
