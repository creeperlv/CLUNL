using CLUNL.Pipeline;
using System.IO;
using System.Threading.Tasks;

namespace CLUNL.DirectedIO
{
    enum PipelineWROption
    {
        WritePipe, ReadPipe
    }

    /// <summary>
    /// Implementation of IBaseWR, used CLUNL.Pipeline to make the data in writer/reader go through pipeline and able to be encrypted or something else.
    /// <br/>All process unit <b>must</b> check the field 'Option' in given PipelineData, it is a PipelineWROption value.
    /// </summary>
    public class PipelineStreamWR : IBaseWR
    {
        IPipelineProcessor WRProcessor;
        Stream stream;
        StreamReader streamReader;
        StreamWriter streamWriter;
        public long Position { get => stream.Position; set => stream.Position = value; }
        public bool AutoFlush { get => streamWriter.AutoFlush; set => streamWriter.AutoFlush = value; }
        public PipelineStreamWR(Stream stream, IPipelineProcessor processor)
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
            byte[] b = new byte[length];
            stream.Read(b, offset, length);
            PipelineData pipelineData = new PipelineData(b, (length, offset), PipelineWROption.ReadPipe);
            var Result = WRProcessor.Process(pipelineData);
            return (byte[])Result.PrimaryData;
        }

        public char ReadChar()
        {
            char c = (char)streamReader.Read();
            PipelineData pipelineData = new PipelineData(c, null, PipelineWROption.ReadPipe);
            var result = WRProcessor.Process(pipelineData);
            return (char)result.PrimaryData;
        }

        public string ReadLine()
        {
            string Line = streamReader.ReadLine();
            PipelineData pipelineData = new PipelineData(Line, null, PipelineWROption.ReadPipe);
            var result = WRProcessor.Process(pipelineData);
            return (string)result.PrimaryData;
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

        public async Task WriteBytesAsync(byte[] b, int length, int offset)
        {
            (int, int) PacketInfo = (length, offset);
            PipelineData pipelineData = new PipelineData(b, PacketInfo, PipelineWROption.WritePipe);
            var Result = WRProcessor.Process(pipelineData);
            (int, int) ObtainedInfo = ((int, int))Result.SecondaryData;
            await stream.WriteAsync((byte[])Result.PrimaryData, ObtainedInfo.Item1, ObtainedInfo.Item2);
        }

        public void WriteLine(string Str)
        {
            PipelineData pipelineData = new PipelineData(Str, null, PipelineWROption.WritePipe);
            var Result = WRProcessor.Process(pipelineData);
            streamWriter.WriteLine((string)Result.PrimaryData);
        }

        public async Task WriteLineAsync(string str)
        {
            PipelineData pipelineData = new PipelineData(str, null, PipelineWROption.WritePipe);
            var Result = WRProcessor.Process(pipelineData);
            await streamWriter.WriteLineAsync((string)Result.PrimaryData);
        }
        /// <summary>
        /// Get or set the length of current stream.
        /// </summary>
        public long Length { get => stream.Length; set => SetLength(value); }
    }
}
