using System;
using System.Collections;

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

        private Queue<string> _items = new();

        public string LogFilePath { get; private set; }
        public uint Timeout { get; private set; }
        public uint BufferMaxElements { get; private set; }

        private void WriteToFile()
        {
            try
            {
                using (var sw = new StreamWriter(LogFilePath, append:true))
                {
                    while (_items.Count > 0)
                        sw.WriteLine(_items.Dequeue());
                }
            }
            catch
            {
                throw;
            }

        }

        public void Add(string item)
        {
            _items.Enqueue(item);

            if (_items.Count == BufferMaxElements)
                WriteToFile();
        }

    }
}