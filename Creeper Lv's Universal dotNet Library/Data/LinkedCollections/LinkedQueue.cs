using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CLUNL.Data.LinkedCollections
{
    /// <summary>
    /// Implementation of queue in link style.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LinkedQueue<T> : IEnumerable<T>, IEnumerable, IReadOnlyCollection<T>, ICollection
    {

#if PUBLIC_INTERNEL_LINKQUEUE
        public QueueItem First { get; private set; }
#else
        internal QueueItem First { get; set; }
#endif
#if PUBLIC_INTERNEL_LINKQUEUE
        public QueueItem Last { get; private set; }
#else
        internal QueueItem Last { get; set; }
#endif
        /// <summary>
        /// Get the current count of the queue.
        /// </summary>
        public int Count { get; private set; } = 0;
        /// <summary>
        //     Gets a value indicating whether access to the LinkedQueue is synchronized (thread safe).
        /// </summary>
        public bool IsSynchronized => false;
        /// <summary>
        /// Gets an object that can be used to synchronize access to the System.Collections.ICollection.
        /// </summary>
        public object SyncRoot => null;
        /// <summary>
        /// Is the queue read-only?
        /// </summary>
        public bool IsReadOnly => false;

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            var Current = First;
            for (int i = 0; i < Count; i++)
            {
                yield return Current.Item;
                Current = Current.NextItem;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            var Current = First;
            for (int i = 0; i < Count; i++)
            {
                yield return Current.Item;
                Current = Current.NextItem;
            }
        }
        /// <summary>
        /// Add an item to end of the queue.
        /// </summary>
        /// <param name="item"></param>
        public void Add(T item)
        {
            var ITEM = new QueueItem(item);
            if (Count == 0)
            {
                First = ITEM;
            }
            else
            {
                Last.NextItem = ITEM;
            }
            Last = ITEM;
            Count++;

        }
        /// <summary>
        /// Add an item to end of the queue.
        /// </summary>
        /// <param name="item"></param>
        public void Enqueue(T item)
        {
            var ITEM = new QueueItem(item);
            if (Count == 0)
            {
                First = ITEM;
            }
            else
            {
                Last.NextItem = ITEM;
            }
            Last = ITEM;
            Count++;
        }
        /// <summary>
        /// Dequeue the first element of the queue.
        /// </summary>
        /// <returns></returns>
        public T Dequeue()
        {
            if (Count <= 0) throw new ArgumentOutOfRangeException();
            var Out = First;
            First = Out.NextItem;
            Count--;
            return Out.Item;
        }

        /// <summary>
        /// Clear the queue;
        /// </summary>
        public void Clear()
        {
            First = null;
            Last = null;
        }
        bool Contains(QueueItem item, T Target)
        {
            if (item.Item.Equals(Target)) return true;
            else if (Last == item) return false;
            else return Contains(item.NextItem, Target);

        }
        /// <summary>
        /// Determines whether contains given item.
        /// </summary>
        /// <remarks>This method is an O(n) operation, where n is Count.</remarks>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(T item)
        {
            return Contains(First, item);
        }
        /// <summary>
        /// Peek the first element without removing it.
        /// </summary>
        /// <returns></returns>
        public T Peek()
        {
            return First.Item;
        }
        void CopyTo(ref Array array, QueueItem Item, ref int Index)
        {
            array.SetValue(Item.Item, Index);
            Index++;
            CopyTo(ref array, Item.NextItem, ref Index);
        }
        void CopyTo(ref T[] array, QueueItem Item, ref int Index)
        {
            array.SetValue(Item.Item, Index);
            Index++;
            CopyTo(ref array, Item.NextItem, ref Index);
        }
        /// <summary>
        /// Initializes a new array.
        /// </summary>
        /// <returns></returns>
        public T[] ToArray()
        {
            T[] ResultArray = new T[Count];
            int i = 0;
            CopyTo(ref ResultArray, First, ref i);
            return ResultArray;
        }
        ///<summary> 
        /// Copies all elements to an existing one-dimension Array, starting at the specified array index.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        public void CopyTo(Array array, int index)
        {
            CopyTo(ref array, First, ref index);
        }

#if PUBLIC_INTERNEL_LINKQUEUE

        public class QueueItem
        {
            public QueueItem(T Item) { this.Item = Item; }
            public T Item;
            public QueueItem NextItem;
        }
#else
        internal class QueueItem
        {
            internal QueueItem(T Item) { this.Item = Item; }
            internal T Item;
            internal QueueItem NextItem;
        }
#endif

    }
}
