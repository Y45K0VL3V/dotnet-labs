using yakov.ThreadPool;
using System.IO;
using System.Diagnostics;

namespace yakov.Lab1.DirectoryControl
{
    public struct CopyOperationInfo
    {
        public string SrcFilePath;
        public string DestFilePath;
        public uint CopiedFilesAmount;
        public TimeSpan CopyTime;
    }

    public class DirectoryCopier
    {
        public DirectoryCopier(IDynamicThreadPool threadPool)
        {
            _threadPool = threadPool;
        }

        private IDynamicThreadPool _threadPool;

        private uint CopyDirectory(string srcPath, string destPath, bool isDeepCopy)
        {
            var dir = new DirectoryInfo(srcPath);

            if (!dir.Exists)
                throw new DirectoryNotFoundException($"Source directory not found: {dir.FullName}");

            uint copiedFilesAmount = 0;
            DirectoryInfo[] dirs = dir.GetDirectories();

            Directory.CreateDirectory(destPath);

            foreach (FileInfo file in dir.GetFiles())
            {
                string targetFilePath = Path.Combine(destPath, file.Name);
                _threadPool.EnqueueTask(() => file.CopyTo(targetFilePath));
                copiedFilesAmount++;
            }

            if (isDeepCopy)
            {
                foreach (DirectoryInfo subDir in dirs)
                {
                    string newDestinationDir = Path.Combine(destPath, subDir.Name);
                    copiedFilesAmount += CopyDirectory(subDir.FullName, newDestinationDir, true);
                }
            }

            return copiedFilesAmount;
        }

        public CopyOperationInfo CopyTo(string srcPath, string destPath, bool isDeepCopy)
        {
            CopyOperationInfo copyInfo = new() { SrcFilePath = srcPath, DestFilePath = destPath};

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            uint copiedFilesAmount = CopyDirectory(srcPath, destPath, isDeepCopy);
            copyInfo.CopiedFilesAmount = copiedFilesAmount;
            while (!_threadPool.IsAllTasksComplete()) { }

            stopwatch.Stop();
            copyInfo.CopyTime = stopwatch.Elapsed;

            return copyInfo;
        }
    }
}
