using System;
using System.Threading.Tasks;

namespace CLUNL.DirectedIO
{
    /// <summary>
    /// Universal interface to R.
    /// </summary>
    public interface IBaseReader : IDisposable
    {
        /// <summary>
        /// Should return a null array when encounter the end of current object.
        /// </summary>
        /// <param name="length"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        byte[] Read(int length, int offset);
        /// <summary>
        /// Read a line.
        /// </summary>
        /// <returns></returns>
        string ReadLine();
        /// <summary>
        /// Read a char.
        /// </summary>
        /// <returns></returns>
        char ReadChar();
        /// <summary>
        /// Get or set the position of current R.
        /// </summary>
        long Position { get; set; }
    }
    /// <summary>
    /// Universal interface to W.
    /// </summary>
    public interface IBaseWriter : IDisposable
    {
        /// <summary>
        /// Whether W should auto flush when writed data to operating data subject.
        /// </summary>
        bool AutoFlush { get; set; }
        /// <summary>
        /// Get or set the lenght of current W.
        /// </summary>
        long Length { get; set; }
        /// <summary>
        /// Write byte array.
        /// </summary>
        /// <param name="b"></param>
        /// <param name="length"></param>
        /// <param name="offset"></param>
        void WriteBytes(byte[] b, int length, int offset);
        /// <summary>
        /// Write byte array asynchronously.
        /// </summary>
        /// <param name="b"></param>
        /// <param name="length"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        Task WriteBytesAsync(byte[] b, int length, int offset);
        /// <summary>
        /// Write a line.
        /// </summary>
        /// <param name="Str"></param>
        void WriteLine(string Str);
        /// <summary>
        /// Write a string.
        /// </summary>
        /// <param name="Str"></param>
        void Write(string Str);
        /// <summary>
        /// Write a char.
        /// </summary>
        /// <param name="c"></param>
        void Write(char c);
        /// <summary>
        /// Write a char asynchronously.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        Task WriteAsync(char c);
        /// <summary>
        /// Write a string asynchronously.
        /// </summary>
        /// <param name="Str"></param>
        /// <returns></returns>
        Task WriteAsync(string Str);
        /// <summary>
        /// Write a line asynchronously.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        Task WriteLineAsync(String str);
        /// <summary>
        /// Flush W.
        /// </summary>
        void Flush();
        /// <summary>
        /// Flush W asynchronously.
        /// </summary>
        /// <returns></returns>
        Task FlushAsync();
        /// <summary>
        /// Set the length of W.
        /// </summary>
        /// <param name="Length"></param>
        void SetLength(long Length);
        /// <summary>
        /// Get or set the position of W.
        /// </summary>
        long Position { get; set; }
    }
    /// <summary>
    /// Combination of IBaerReader and IBaseWriter.
    /// </summary>
    public interface IBaseWR : IBaseWriter, IBaseReader
    {
        /// <summary>
        /// Hide original Position from both W/R.
        /// </summary>
        new long Position { get; set; }
    }
}
