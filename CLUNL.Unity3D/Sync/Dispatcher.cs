using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
//using System.Collections.Immutable;
using System.Runtime.CompilerServices;

namespace CLUNL.Unity3D.Sync
{
    public class Dispatcher
    {
        public static int MaxSingleInvokesInSingleFrame = -1;
        //static ImmutableQueue<Action> iactions = ImmutableQueue<Action>.Empty;
        static ConcurrentQueue<KeyValuePair<Action, int>> actions = new ConcurrentQueue<KeyValuePair<Action, int>>();
        static object _lock = new object();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Inited()
        {
            return init;
        }
        static bool init = false;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void MarkInit()
        {
            init = true;
        }
        /// <summary>
        /// Call to perform a sync operation every frame.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Sync()
        {
            lock (_lock)
            {
                int i = 0;
                while (actions.Count > 0)
                {
                    if (actions.TryDequeue(out KeyValuePair<Action, int> K))
                    {
                        if (MaxSingleInvokesInSingleFrame == -1)
                        {
                            K.Key();
                        }
                        else
                        {
                            if (i < MaxSingleInvokesInSingleFrame)
                            {
                                K.Key();
                                i++;
                            }
                            else
                            {
                                if (K.Value - 1 != -1)
                                    Invoke(K.Key, K.Value - 1);
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Enqueue an action from non-main thread.
        /// </summary>
        /// <param name="action"></param>
        /// <param name="Frame">Max frame that the action can survive until being ignored. Value less than -1 means it will never be dropped.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Invoke(Action action, int Frame = -2)
        {
            lock (_lock)
            {
                actions.Enqueue(new KeyValuePair<Action, int>(action, Frame));
            }
        }
    }
}
