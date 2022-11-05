using System;
using System.Collections;
using System.Diagnostics;
using yakov.ThreadPool;

namespace yakov.Logger
{
    public sealed class LogBuffer
    {
        public LogBuffer(string logFilePath, uint timeout, uint bufMaxElements)
        {
            LogFilePath = logFilePath;
            Timeout = timeout;
            BufferMaxElements = bufMaxElements;

            _writeTimer = new Timer(new(WriteToFileTimer), null, 0, Timeout);
        }

        private Mutex _mutex = new();
        private TaskQueue _threadPool = new(1);
        private Timer _writeTimer;
        private Queue<string> _items = new();

        public string LogFilePath { get; private set; }
        public uint Timeout { get; private set; }
        public uint BufferMaxElements { get; private set; }

        private void WriteToFileTimer(object? obj)
        {
            WriteToFile();
        }

        private async void WriteToFile()
        {
            try
            {
                _mutex.WaitOne();
                Queue<string> itemsToWrite = new(_items);
                _items = new();
                _mutex.ReleaseMutex();

                await Task.Run(() =>
                {
                    _threadPool.EnqueueTask(() => File.AppendAllLines(LogFilePath, itemsToWrite));
                });
            }
            catch { }
        }

        public void Add(string item)
        {
            _items.Enqueue(item);

            if (_items.Count == BufferMaxElements)
                WriteToFile();
        }

    }
}