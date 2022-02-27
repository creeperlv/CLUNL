using CLUNL.Data.Serializables.SSS;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CLUNLTests.SSSTests
{

    [TestClass]
    public class SSSTest
    {
        [TestMethod]
        public void Test0()
        {
            List<SSS_Base> list = new();
            Random random = new Random();
            for (int i = 0; i < 50; i++)
            {
                int index = i % 4;
                switch (index)
                {
                    case 0:
                        {
                            list.Add(new SSS_000 { D00 = random.Next() });
                        }
                        break;
                    case 1:
                        {
                            list.Add(new SSS_001 { D00 = random.Next() % 2 == 1 });
                        }
                        break;
                    case 2:
                        {
                            list.Add(new SSS_002 { D00 = random.Next() / 100f });
                        }
                        break;
                    case 3:
                        {
                            list.Add(new SSS_003 { D00 = random.NextDouble() });
                        }
                        break;
                    default:
                        break;
                }
            }
            var content = Serializer.Serialize(list);
            foreach (var item in content)
            {
                Trace.WriteLine(item);
            }
            var result = Deserializer.Deserialize<SSS_Base>(content);
            foreach (var item in result)
            {
                Trace.WriteLine(item);

            }
        }
    }
    public class SSS_Base
    {
    }
    public class SSS_000 : SSS_Base
    {
        public int D00;
        public override string ToString()
        {
            return $"{this.GetType().Name}:{D00}";
        }
    }
    public class SSS_001 : SSS_Base
    {
        public bool D00; public override string ToString()
        {
            return $"{this.GetType().Name}:{D00}";
        }
    }
    public class SSS_002 : SSS_Base
    {
        public float D00; public override string ToString()
        {
            return $"{this.GetType().Name}:{D00}";
        }
    }
    public class SSS_003 : SSS_Base
    {
        public double D00; public override string ToString()
        {
            return $"{this.GetType().Name}:{D00}";
        }
    }
}
