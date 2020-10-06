using CLUNL.Data.Layer0.Buffers;
using CLUNL.Data.LinkedCollections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using System.Text;

namespace CLUNLTests
{
    [TestClass]
    public class BufferTest
    {
        [TestMethod]
        public void ByteBufferTest0()
        {
            ByteBuffer vs = new ByteBuffer();
            Random random = new Random();
            for (int i = 0; i < 1000000; i++)
            {
                vs.AppendGroup(BitConverter.GetBytes(
                random.Next()));
            }
        }
        [TestMethod]
        public void LinkedQueueTest()
        {
            Random random = new Random();
            LinkedQueue<int> q = new LinkedQueue<int>();
            for (int i = 0; i < 1000000; i++)
            {
                q.Add(random.Next());
                q.Dequeue();
                q.Add(random.Next());
                q.Dequeue();
                q.Add(random.Next());
                q.Dequeue();
                q.Add(random.Next());
            }
            for (int i = 0; i < 1000000; i++)
            {
                q.Add(random.Next());
                q.Dequeue();
                q.Add(random.Next());
                q.Dequeue();
                q.Add(random.Next());
                q.Dequeue();
                q.Add(random.Next());
            }
            for (int i = 0; i < 1000000; i++)
            {
                q.Add(random.Next());
                q.Dequeue();
                q.Add(random.Next());
                q.Dequeue();
                q.Add(random.Next());
                q.Dequeue();
                q.Add(random.Next());
            }

        }

        [TestMethod]
        public void BaseQueueTest()
        {
            Random random = new Random();
            Queue<int> q2 = new Queue<int>();
            for (int i = 0; i < 1000000; i++)
            {
                q2.Enqueue(random.Next());
                q2.Dequeue();
                q2.Enqueue(random.Next());
                q2.Dequeue();
                q2.Enqueue(random.Next());
                q2.Dequeue();
                q2.Enqueue(random.Next());
            }
            for (int i = 0; i < 1000000; i++)
            {
                q2.Enqueue(random.Next());
                q2.Dequeue();
                q2.Enqueue(random.Next());
                q2.Dequeue();
                q2.Enqueue(random.Next());
                q2.Dequeue();
                q2.Enqueue(random.Next());
            }
            for (int i = 0; i < 1000000; i++)
            {
                q2.Enqueue(random.Next());
                q2.Dequeue();
                q2.Enqueue(random.Next());
                q2.Dequeue();
                q2.Enqueue(random.Next());
                q2.Dequeue();
                q2.Enqueue(random.Next());
            }

            //for (int i = 0; i < 50000; i++)
            //{
            //}
            //for (int i = 0; i < 1000000; i++)
            //{
            //}
        }
}
