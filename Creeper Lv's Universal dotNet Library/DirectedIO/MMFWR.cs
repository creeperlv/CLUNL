using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CLUNL.DirectedIO
{
    public class MMFWR : IBaseWR
    {
        public long Position { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool AutoFlush { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Flush()
        {
            throw new NotImplementedException();
        }

        public Task FlushAsync()
        {
            throw new NotImplementedException();
        }

        public byte[] Read(int length, int offset)
        {
            throw new NotImplementedException();
        }

        public char ReadChar()
        {
            throw new NotImplementedException();
        }

        public string ReadLine()
        {
            throw new NotImplementedException();
        }

        public void SetLength(long Length)
        {
            throw new NotImplementedException();
        }

        public void Write(string Str)
        {
            throw new NotImplementedException();
        }

        public void Write(char c)
        {
            throw new NotImplementedException();
        }

        public Task WriteAsync(char c)
        {
            throw new NotImplementedException();
        }

        public Task WriteAsync(string Str)
        {
            throw new NotImplementedException();
        }

        public void WriteBytes(byte[] b, int length, int offset)
        {
            throw new NotImplementedException();
        }

        public Task WriteBytesAsync(byte[] b, int length, int offset)
        {
            throw new NotImplementedException();
        }

        public void WriteLine(string Str)
        {
            throw new NotImplementedException();
        }

        public Task WriteLineAsync(string str)
        {
            throw new NotImplementedException();
        }
    }
}
