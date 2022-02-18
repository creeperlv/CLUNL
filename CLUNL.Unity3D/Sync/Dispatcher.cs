using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
//using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using System.Text;

namespace CLUNL.Unity3D.Sync
{
    public class Dispatcher
    {
        //static ImmutableQueue<Action> iactions = ImmutableQueue<Action>.Empty;
        static ConcurrentQueue<Action> actions = new ConcurrentQueue<Action>();
        static object _lock = new object();
        /// <summary>
        /// Call to perform a sync operation every frame.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Sync()
        {
            lock (_lock)
            {
                while (actions.Count > 0)
                {
                    if (actions.TryDequeue(out Action action))
                    {
                        action();
                    }
                }
            }
        }
        /// <summary>
        /// Enqueue an action from non-main thread.
        /// </summary>
        /// <param name="action"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Invoke(Action action)
        {
            lock (_lock)
            {
                actions.Enqueue(action);
            }
        }
    }
}
