using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CLUNL.DirectedIO
{
    /// <summary>
    /// A WR Implementation to operate stream.
    /// </summary>
    public class StreamWR : IBaseWR
    {
        Stream Stream;
        StreamReader reader;
        StreamWriter writer;
        /// <summary>
        /// Initialize the WR from given Stream.
        /// </summary>
        /// <param name="stream"></param>
        public StreamWR(Stream stream)
        {
            Stream = stream;
            reader = new StreamReader(Stream);
            writer = new StreamWriter(Stream);
        }
        /// <summary>
        /// Set the position within the operating stream.
        /// </summary>
        public long Position { get => Stream.Position; set { Stream.Position = value; } }
        /// <summary>
        /// Set whether current WR will do flush after written something.
        /// </summary>
        public bool AutoFlush { get => writer.AutoFlush; set => writer.AutoFlush = value; }
        /// <summary>
        /// Relaease operating stream.
        /// </summary>
        public void Dispose()
        {
            writer.Close();
            reader.Close();
            Stream.Dispose();
        }
        /// <summary>
        /// Clears buffers of current WR and all buffered data to be written to operating stream.
        /// </summary>
        public void Flush()
        {
            writer.Flush();
        }
        /// <summary>
        /// Clears buffers of current WR asynchronously and all buffered data to be written to operating stream.
        /// </summary>
        /// <returns></returns>
        public async Task FlushAsync()
        {
            await writer.FlushAsync();
        }
        /// <summary>
        /// Reads a byte array in given length from given offset. Will return a null byte array if it reaches the end of the stream. (When Stream.Read()==0)
        /// </summary>
        /// <param name="length"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public byte[] Read(int length, int offset)
        {
            byte[] b = (byte[])Array.CreateInstance(typeof(byte), length);
            if (Stream.Read(b, offset, length) == 0) return null;
            return b;
        }
        /// <summary>
        /// Read a char.
        /// </summary>
        /// <returns></returns>
        public char ReadChar()
        {
            return (char)reader.Read();
        }
        /// <summary>
        /// Read a line.
        /// </summary>
        /// <returns></returns>
        public string ReadLine()
        {
            return reader.ReadLine();
        }
        /// <summary>
        /// Set the length of operating stream.
        /// </summary>
        /// <param name="Length"></param>
        public void SetLength(long Length)
        {
            writer.BaseStream.SetLength(Length);
        }
        /// <summary>
        /// Write a string.
        /// </summary>
        /// <param name="Str"></param>
        public void Write(string Str)
        {
            writer.Write(Str);
        }
        /// <summary>
        /// Write a char.
        /// </summary>
        /// <param name="c"></param>
        public void Write(char c)
        {
            writer.Write(c);
        }
        /// <summary>
        /// Write a string asynchronously.
        /// </summary>
        /// <param name="Str"></param>
        /// <returns></returns>
        public async Task WriteAsync(string Str)
        {
            await writer.WriteAsync(Str);
        }
        /// <summary>
        /// Write a char asynchronously.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public async Task WriteAsync(char c)
        {
            await writer.WriteAsync(c);
        }
        /// <summary>
        /// Write a byte array.
        /// </summary>
        /// <param name="b"></param>
        /// <param name="length"></param>
        /// <param name="offset"></param>
        public void WriteBytes(byte[] b, int length, int offset)
        {
            writer.BaseStream.Write(b, offset, length);
        }
        /// <summary>
        /// Write a byte array asynchronously.
        /// </summary>
        /// <param name="b"></param>
        /// <param name="length"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public async Task WriteBytesAsync(byte[] b, int length, int offset)
        {
            await writer.BaseStream.WriteAsync(b, offset, length);
        }
        /// <summary>
        /// Write a line.
        /// </summary>
        /// <param name="Str"></param>
        public void WriteLine(string Str)
        {
            writer.WriteLine(Str);
        }
        /// <summary>
        /// Write a line asynchronously.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public async Task WriteLineAsync(string str)
        {
            await writer.WriteLineAsync(str);
        }
        /// <summary>
        /// Get or set the length of current stream.
        /// </summary>
        public long Length { get => Stream.Length; set => SetLength(value); }
    }
}
