using System;
using System.Threading.Tasks;

namespace CLUNL.DirectedIO
{
    public interface IBaseReader: IDisposable
    {
        byte[] Read(int length, int offset);
        string ReadLine();
        char ReadChar();
        long Position { get; set; }
    }
    public interface IBaseWriter : IDisposable
    {
        bool AutoFlush { get; set; }
        void WriteBytes(byte[] b, int length, int offset);
        Task WriteBytesAsync(byte[] b, int length, int offset);
        void WriteLine(string Str);
        void Write(string Str);
        void Write(char c);
        Task WriteAsync(char c);
        Task WriteAsync(string Str);
        Task WriteLineAsync(String str);
        void Flush();
        Task FlushAsync();
        void SetLength(long Length);
        long Position { get; set; }
    }
    public interface IBaseWR : IBaseWriter, IBaseReader
    {
        new long Position { get; set; }
    }
}
