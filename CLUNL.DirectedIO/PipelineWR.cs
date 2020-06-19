using CLUNL.Pipeline;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CLUNL.DirectedIO
{
    enum PipelineWROption
    {
        WritePipe, ReadPipe
    }
    public class PipelineWR : IBaseWR
    {
        IPipelineProcessor WRProcessor;
        Stream stream;
        StreamReader streamReader;
        StreamWriter streamWriter;
        public long Position { get => stream.Position; set => stream.Position=value; }
        public bool AutoFlush { get => streamWriter.AutoFlush; set => streamWriter.AutoFlush=value; }
        public PipelineWR(Stream stream, IPipelineProcessor processor)
        {
            this.stream = stream;
            streamWriter = new StreamWriter(stream);
            streamReader = new StreamReader(stream);
            WRProcessor = processor;
        }
        public void Dispose()
        {
            streamReader.Close();
            streamWriter.Close();
            stream.Dispose();
        }

        public void Flush()
        {
            stream.Flush();
        }

        public async Task FlushAsync()
        {
            await stream.FlushAsync();
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
            stream.SetLength(Length);
        }

        public void Write(string Str)
        {
            PipelineData pipelineData = new PipelineData(Str, null, PipelineWROption.WritePipe);
            var Result = WRProcessor.Process(pipelineData);
            streamWriter.Write((string)Result.PrimaryData);
        }

        public void Write(char c)
        {
            PipelineData pipelineData = new PipelineData(c, null, PipelineWROption.WritePipe);
            var Result = WRProcessor.Process(pipelineData);
            streamWriter.Write((char)Result.PrimaryData);
        }

        public async Task WriteAsync(char c)
        {
            PipelineData pipelineData = new PipelineData(c, null, PipelineWROption.WritePipe);
            var Result = WRProcessor.Process(pipelineData);
            await streamWriter.WriteAsync((char)Result.PrimaryData);
        }

        public async Task WriteAsync(string Str)
        {
            PipelineData pipelineData = new PipelineData(Str, null, PipelineWROption.WritePipe);
            var Result = WRProcessor.Process(pipelineData);
            await streamWriter.WriteAsync((string)Result.PrimaryData);
        }

        public void WriteBytes(byte[] b, int length, int offset)
        {
            (int, int) PacketInfo = (length, offset);
            PipelineData pipelineData = new PipelineData(b, PacketInfo, PipelineWROption.WritePipe);
            var Result = WRProcessor.Process(pipelineData);
            (int, int) ObtainedInfo = ((int, int))Result.SecondaryData;
            stream.Write((byte[])Result.PrimaryData, ObtainedInfo.Item1, ObtainedInfo.Item2);
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
