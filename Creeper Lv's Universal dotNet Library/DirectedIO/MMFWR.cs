using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace CLUNL.DirectedIO
{
    /// <summary>
    /// MMF - Memory Mapped File. Operates MMF.
    /// </summary>
    public class MMFWR : IBaseWR
    {
        MemoryMappedFile file;
        Stream MemoryFileStream;
        StreamReader StreamReader;
        StreamWriter StreamWriter;
        public MemoryMappedFile CoreFile
        {
            get => file;private set
            {
                file = value;
                MemoryFileStream = file.CreateViewStream();
                StreamWriter = new StreamWriter(MemoryFileStream);
                StreamReader = new StreamReader(MemoryFileStream);
            }
        }
        /// <summary>
        /// Creates a new memory mapped file with given length.
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Capacity"></param>
        /// <returns></returns>
        public static MMFWR CreateNew(string Name, long Capacity)
        {
            MMFWR mMFWR = new MMFWR();
            mMFWR.CoreFile = MemoryMappedFile.CreateNew(Name, Capacity);
            return mMFWR;
        }
        /// <summary>
        /// Opens an existing memory mapped file.
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public static MMFWR Open(string Name)
        {
            MMFWR WR = new MMFWR();
            WR.CoreFile = MemoryMappedFile.OpenExisting(Name);
            return WR;
        }
        public long Position { get => MemoryFileStream.Position; set => MemoryFileStream.Position = value; }
        public bool AutoFlush { get; set; }
        /// <summary>
        /// Releases operating resources.
        /// </summary>
        public void Dispose()
        {
            CoreFile.Dispose();
        }
        /// <summary>
        /// Flush the cache to target MMF.
        /// </summary>
        public void Flush()
        {
            MemoryFileStream.Flush();
        }
        /// <summary>
        /// Flush the cache to target MMF asynchronously.
        /// </summary>
        /// <returns></returns>
        public async Task FlushAsync()
        {
            await MemoryFileStream.FlushAsync();
        }
        /// <summary>
        /// Read a byte array. Will return a null byte array if it reaches the end of the MMF.
        /// </summary>
        /// <param name="length"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public byte[] Read(int length, int offset)
        {
            byte[] b = new byte[length];
            if (MemoryFileStream.Read(b, offset, length) == 0) return null;
            return b;
        }
        /// <summary>
        /// Read a char.
        /// </summary>
        /// <returns></returns>
        public char ReadChar()
        {
            return (char)StreamReader.Read();
        }
        /// <summary>
        /// Read a line.
        /// </summary>
        /// <returns></returns>
        public string ReadLine()
        {
            return StreamReader.ReadLine();
        }
        /// <summary>
        /// Set the length of the Stream.
        /// </summary>
        /// <param name="Length"></param>
        public void SetLength(long Length)
        {
            MemoryFileStream.SetLength(Length);
        }
        /// <summary>
        /// Write a string.
        /// </summary>
        /// <param name="Str"></param>
        public void Write(string Str)
        {
            StreamWriter.Write(Str);
        }
        /// <summary>
        /// Write a char.
        /// </summary>
        /// <param name="c"></param>
        public void Write(char c)
        {
            StreamWriter.Write(c);
        }
        /// <summary>
        /// Write a char asynchronously.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public async Task WriteAsync(char c)
        {
            await StreamWriter.WriteAsync(c);
        }
        /// <summary>
        /// Write a string asynchronously.
        /// </summary>
        /// <param name="Str"></param>
        /// <returns></returns>
        public async Task WriteAsync(string Str)
        {
            await StreamWriter.WriteAsync(Str);
        }
        /// <summary>
        /// Write a byte array.
        /// </summary>
        /// <param name="b"></param>
        /// <param name="length"></param>
        /// <param name="offset"></param>
        public void WriteBytes(byte[] b, int length, int offset)
        {
            MemoryFileStream.Write(b, offset, length);
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
            await MemoryFileStream.WriteAsync(b, offset, length);
        }
        /// <summary>
        /// Write a string as a line.
        /// </summary>
        /// <param name="Str"></param>
        public void WriteLine(string Str)
        {
            StreamWriter.WriteLine(Str);
        }
        /// <summary>
        /// Write a string as a line asynchronously.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public async Task WriteLineAsync(string str)
        {
            await StreamWriter.WriteLineAsync(str);
        }
    }
}
