using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CLUNL.DirectedIO
{
    public class MemoryWR : IBaseWR
    {
        MemoryStream MemoryStream;
        StreamReader StreamReader;
        StreamWriter StreamWriter;
        public MemoryWR(byte[] BaseData)
        {
            MemoryStream = new MemoryStream(BaseData);
            StreamWriter = new StreamWriter(MemoryStream);
            StreamReader = new StreamReader(MemoryStream);
        }
        public long Position { get => MemoryStream.Position; set => MemoryStream.Position = value; }
        public bool AutoFlush { get => StreamWriter.AutoFlush; set => StreamWriter.AutoFlush = value; }

        public void Dispose()
        {

            StreamReader.Close();
            StreamWriter.Close();
            MemoryStream.Close();
        }

        public void Flush()
        {
            StreamWriter.Flush();
        }

        public async Task FlushAsync()
        {
            await StreamWriter.FlushAsync();
            await MemoryStream.FlushAsync();
        }
        public byte[] GetAllData()
        {
            return MemoryStream.ToArray();
        }
        public byte[] Read(int length, int offset)
        {
            byte[] vs = new byte[length];
            MemoryStream.Read(vs, offset, length);
            return vs;
        }

        public char ReadChar()
        {
            return (char)StreamReader.Read();
        }

        public string ReadLine()
        {
            return StreamReader.ReadLine();
        }

        public void SetLength(long Length)
        {
            MemoryStream.SetLength(Length);
        }

        public void Write(string Str)
        {
            StreamWriter.Write(Str);
        }

        public void Write(char c)
        {
            StreamWriter.Write(c);
        }

        public async Task WriteAsync(char c)
        {
            await StreamWriter.WriteAsync(c);
        }

        public async Task WriteAsync(string Str)
        {
            await StreamWriter.WriteAsync(Str);
        }

        public void WriteBytes(byte[] b, int length, int offset)
        {
            MemoryStream.Write(b, offset, length);
        }

        public async Task WriteBytesAsync(byte[] b, int length, int offset)
        {
            await MemoryStream.WriteAsync(b, offset, length); 
        }

        public void WriteLine(string Str)
        {
            StreamWriter.WriteLine(Str);
        }

        public async Task WriteLineAsync(string str)
        {
            await StreamWriter.WriteLineAsync(str);
        }
        /// <summary>
        /// Get or set the length of current memory block.
        /// </summary>
        public long Length { get => MemoryStream.Length; set => SetLength(value); }
    }
}
