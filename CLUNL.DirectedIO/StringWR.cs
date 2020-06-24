using CLUNL.Massives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CLUNL.DirectedIO
{
    public class StringWR : HoldableObject, IBaseWR
    {
        MemoryStream memoryStream;
        StreamReader reader;
        StreamWriter writer;
        public StringWR(string Origin)
        {
            memoryStream = new MemoryStream(Encoding.Default.GetBytes(Origin));
            reader = new StreamReader(memoryStream);
            writer = new StreamWriter(memoryStream);
        }
        public bool AutoFlush { get => writer.AutoFlush; set => writer.AutoFlush = value; }
        public long Position
        {
            get => memoryStream.Position; set
            {
                memoryStream.Position = value;
            }
        }

        public void Dispose()
        {
            writer.Close();
            reader.Close();
        }

        public void Flush()
        {
            writer.Flush();
        }

        public async Task FlushAsync()
        {
            await writer.FlushAsync();
        }

        public byte[] Read(int length, int offset)
        {
            byte[] b = (byte[])Array.CreateInstance(typeof(byte), length);
            memoryStream.Read(b, offset, length);
            return b;
        }

        public char ReadChar()
        {
            return (char)reader.Read();
        }
        public string GetAllString()
        {
            return Encoding.Default.GetString(memoryStream.ToArray());
        }
        public string GetAllString(Encoding encoding)
        {
            return encoding.GetString(memoryStream.ToArray());
        }
        public string ReadLine()
        {
            return reader.ReadLine();
        }

        public void SetLength(long Length)
        {
            writer.BaseStream.SetLength(Length);
        }

        public void Write(string Str)
        {
            writer.Write(Str);
        }

        public void Write(char c)
        {
            writer.Write(c);
        }

        public async Task WriteAsync(string Str)
        {
            await writer.WriteAsync(Str);
        }
        public async Task WriteAsync(char c)
        {
            await writer.WriteAsync(c);
        }

        public void WriteBytes(byte[] b, int length, int offset)
        {
            writer.BaseStream.Write(b, offset, length);
        }

        public async Task WriteBytesAsync(byte[] b, int length, int offset)
        {
            await writer.BaseStream.WriteAsync(b, offset, length);
        }

        public void WriteLine(string Str)
        {
            writer.WriteLine(Str);
        }

        public async Task WriteLineAsync(string str)
        {
            await writer.WriteLineAsync(str);
        }
    }
}
