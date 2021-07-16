﻿using CLUNL.Data.Serializables.CheckpointSystem;
using CLUNL.Data.Serializables.CheckpointSystem.Types;
using CLUNL.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLUNLTests
{
    [TestClass]
    public class CheckPointSystemTest
    {
        [TestMethod]
        public void SaveAndLoadTest()
        {
            CheckpointSystem.Init((new DirectoryInfo(".").FullName));

            var CP=CheckpointSystem.CurrentCheckpointSystem.GetOrCreateCheckPoint("TestCP");
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
                    CP.RegisterCheckPointData(data);
                    CPData.Add(data);
                }
                {
                    Data00 data = new Data00();
                    data.Name = $"D{i}";
                    data.Field00 = i % 10;
                    data.Field01 = i * 10;
                    data.Field02 = str;
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
        public void Load(List<object> data)
        {
            Field00 = (IntNumber)data[0];
            Field01 = (IntNumber)data[1];
            Field02 = (string)data[2];
        }
        public override bool Equals(object obj)
        {
            if(obj is Data00 d)
            {
                return Field00 == d.Field00 && Field01 == d.Field01 && Field02 == d.Field02;
            }
            return base.Equals(obj);
        }

        public List<object> Save()
        {
            return new List<object> { Field00, Field01, Field02 };
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
