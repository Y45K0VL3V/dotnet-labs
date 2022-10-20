using System;

namespace yakov.Logger
{
    public sealed class LogBuffer
    {
        public LogBuffer(string logFilePath, uint timeout, uint bufMaxElements)
        {
            LogFilePath = logFilePath;
            Timeout = timeout;
            BufferMaxElements = bufMaxElements;
        }

        private List<string> _items = new();

        public string LogFilePath { get; private set; }
        public uint Timeout { get; private set; }
        public uint BufferMaxElements { get; private set; }

        private void WriteToFile()
        {
            throw new NotImplementedException();
        }

        public void Add(string item)
        {
            throw new NotImplementedException();
        }

    }
}