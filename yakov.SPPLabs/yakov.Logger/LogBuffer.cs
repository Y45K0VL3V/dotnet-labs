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

            _writeTimer = new Timer(new(WriteToFile), _items, 0, Timeout);
        }

        private readonly Timer _writeTimer;
        private Queue<string> _items = new();

        public string LogFilePath { get; private set; }
        public uint Timeout { get; private set; }
        public uint BufferMaxElements { get; private set; }

        private async void WriteToFile(object? itemsObj)
        {
            _items = new();
            await Task.Run(() =>
            {
                try
                {
                    Queue<string>? items = itemsObj as Queue<string>;
                    if (items == null)
                        return;

                    using (var sw = new StreamWriter(LogFilePath, append: true))
                    {
                        while (items.Count > 0)
                            sw.WriteLine(items.Dequeue());
                    }
                }
                catch { }
            });
        }

        public void Add(string item)
        {
            _items.Enqueue(item);

            if (_items.Count == BufferMaxElements)
                WriteToFile(_items);
        }

    }
}