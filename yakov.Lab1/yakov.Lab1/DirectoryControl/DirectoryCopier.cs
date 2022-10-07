using yakov.ThreadPool;
using System.IO;

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

        public CopyOperationInfo? LastOperationInfo;

        private void CopyDirectory(string srcPath, string destPath, bool isDeepCopy)
        {
            var dir = new DirectoryInfo(srcPath);

            if (!dir.Exists)
                throw new DirectoryNotFoundException($"Source directory not found: {dir.FullName}");

            DirectoryInfo[] dirs = dir.GetDirectories();

            Directory.CreateDirectory(destPath);

            foreach (FileInfo file in dir.GetFiles())
            {
                string targetFilePath = Path.Combine(destPath, file.Name);
                file.CopyTo(targetFilePath);
            }

            if (isDeepCopy)
            {
                foreach (DirectoryInfo subDir in dirs)
                {
                    string newDestinationDir = Path.Combine(destPath, subDir.Name);
                    CopyDirectory(subDir.FullName, newDestinationDir, true);
                }
            }
        }

        public void CopyTo(string srcPath, string destPath, bool isDeepCopy)
        {
            CopyDirectory(srcPath, destPath, isDeepCopy);
        }
    }
}
