using CLUNL.Data.Serializables.CheckpointSystem;
using CLUNL.Data.Serializables.CheckpointSystem.Types;
using CLUNL.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;

namespace CLUNLTests
{
    [TestClass]
    public class CheckPointSystemTest
    {
        [TestMethod]
        public void SaveAndLoadTest()
        {
            CheckpointSystem.Init((new DirectoryInfo(".").FullName));

            var CP = CheckpointSystem.CurrentCheckpointSystem.GetOrCreateCheckPoint("TestCP");
            List<Data00> OriginalData = new List<Data00>();
            List<Data00> CPData = new List<Data00>();
            for (int i = 0; i < 100; i++)
            {
                var str = RandomTool.GetRandomString(10, RandomStringRange.R3);
                {
                    Data00 data = new Data00();
                    data.Name = $"D{i}";
                    data.Field00 = i % 10;
                    data.Field01 = i * 10;
                    data.Field02 = str;
                    data.Field03 = i / 10f;
                    data.Field04 = i / 10.0;
                    CP.RegisterCheckPointData(data);
                    CPData.Add(data);
                }
                {
                    Data00 data = new Data00();
                    data.Name = $"D{i}";
                    data.Field00 = i % 10;
                    data.Field01 = i * 10;
                    data.Field02 = str;
                    data.Field03 = i / 10f;
                    data.Field04 = i / 10.0;
                    OriginalData.Add(data);
                }

            }
            CP.LoadSnapshot(CP.CreateSnapshot());
            for (int i = 0; i < 100; i++)
            {
                Assert.AreEqual(CPData[i], OriginalData[i]);
            }
        }
    }
    public class Data00 : ICheckpointData
    {
        public string Name;
        public string GetName() => Name;
        public IntNumber Field00;
        public IntNumber Field01;
        public string Field02;
        public FloatNumber Field03;
        public double Field04;
        public void Load(List<object> data)
        {
            Field00 = (IntNumber)data[0];
            Field01 = (IntNumber)data[1];
            Field02 = (string)data[2];
            Field03 = (FloatNumber)data[3];
            Field04 = (double)data[4];
        }
        public override bool Equals(object obj)
        {
            if (obj is Data00 d)
            {
                return Field00 == d.Field00 && Field01 == d.Field01 && Field02 == d.Field02 && Field03 == d.Field03 && Field04 == d.Field04;
            }
            return base.Equals(obj);
        }

        public List<object> Save()
        {
            return new List<object> { Field00, Field01, Field02, Field03, Field04 };
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
