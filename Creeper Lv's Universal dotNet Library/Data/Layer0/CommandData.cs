using CLUNL.Data.Layer0.Buffers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CLUNL.Data.Layer0
{
    public struct CommandData : IDataBufferable
    {
        public int Command;
        public string Data;
        public CommandData(int Command, string Data)
        {
            this.Command = Command;
            this.Data = Data;
        }
        public void Deserialize(DataBuffer buffer)
        {
            Command = buffer.ReadInt();
            Data = buffer.ReadString();
        }
        public DataBuffer Serialize()
        {
            DataBuffer buffer = new DataBuffer();
            buffer.WriteInt(Command);
            buffer.WriteString(Data);
            return buffer;
        }

        public override string ToString()
        {
            return string.Format("{0}|{1}", Command.ToString(), Data);
        }

        public static implicit operator string(CommandData D)
        {
            return string.Format("{0}|{1}", D.Command.ToString(), D.Data);
        }
        public static implicit operator DataBuffer(CommandData D)
        {
            return D.Serialize();
        }
        public static implicit operator CommandData(string D)
        {
            var i = D.IndexOf('|');
            if (i > 0)
            {
                var C = D.Substring(0, i);
                var CD = D.Substring(i + 1);
                CommandData commandData = new CommandData(int.Parse(C), CD);
                return commandData;
            }
            else
            {
                throw new InvalidDataException();
            }
        }
        public static implicit operator CommandData(DataBuffer D)
        {
            CommandData commandData = new CommandData();
            commandData.Deserialize(D);
            return commandData;
        }
    }
}
