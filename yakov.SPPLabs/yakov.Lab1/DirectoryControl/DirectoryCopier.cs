using yakov.ThreadPool;
using System.Diagnostics;
using NLog;
using System.Threading;
using System.Xml;

namespace yakov.Lab1.DirectoryControl
{
    /// <summary>
    /// Copy operation info.
    /// </summary>
    public struct CopyOperationInfo
    {
        /// <summary>
        /// Source directory path.
        /// </summary>
        public string SrcPath;
        /// <summary>
        /// Destination directory path.
        /// </summary>
        public string DestPath;
        /// <summary>
        /// Amount of copied files.
        /// </summary>
        public uint CopiedFilesAmount;
        /// <summary>
        /// Time spent on coping.
        /// </summary>
        public TimeSpan CopyTime;
    }

    /// <summary>
    /// Class to copy directories.
    /// </summary>
    public class DirectoryCopier
    {
        /// <summary>
        /// Create copier instance.
        /// </summary>
        /// <param name="threadPool">Thread pool to use.</param>
        public DirectoryCopier(IDynamicThreadPool threadPool)
        {
            _logger.Info($"Directory copier created");
            _threadPool = threadPool;
        }

        /// <summary>
        /// Logger.
        /// </summary>
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        /// <summary>
        /// Thread pool to invoke file copy tasks.
        /// </summary>
        private IDynamicThreadPool _threadPool;

        /// <summary>
        /// Copy directory mechanism.
        /// </summary>
        /// <param name="srcPath">Directory source path.</param>
        /// <param name="destPath">Directory destination path.</param>
        /// <param name="isDeepCopy">Is need to copy subdirectories.</param>
        /// <returns></returns>
        /// <exception cref="DirectoryNotFoundException">Throws if directory not exist with given path.</exception>
        private uint CopyDirectory(string srcPath, string destPath, bool isDeepCopy)
        {
            var dir = new DirectoryInfo(srcPath);

            if (!dir.Exists)
            {
                _logger.Error($"Source directory not found: {dir.FullName}");
                throw new DirectoryNotFoundException($"Source directory not found: {dir.FullName}.");
            }

            uint copiedFilesAmount = 0;
            DirectoryInfo[] dirs = dir.GetDirectories();

            Directory.CreateDirectory(destPath);

            foreach (FileInfo file in dir.GetFiles())
            {
                string targetFilePath = Path.Combine(destPath, file.Name);

                try
                {
                    _threadPool.EnqueueTask(() => 
                    {
                        _logger.Info($"{Thread.CurrentThread.ManagedThreadId} thread - start copy {file.FullName} to {targetFilePath}.");
                        file.CopyTo(targetFilePath);
                        _logger.Info($"{Thread.CurrentThread.ManagedThreadId} thread - end copy {file.FullName} to {targetFilePath}.");
                    });
                    copiedFilesAmount++;
                }
                catch
                {
                    _logger.Error($"Can not be copied to {targetFilePath}.");
                }
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

        /// <summary>
        /// Copy directory.
        /// </summary>
        /// <param name="srcPath">Directory source absolute path.</param>
        /// <param name="destPath">Directory destination absolute path.</param>
        /// <param name="isDeepCopy">Is need to copy subdirectories.</param>
        /// <returns>Copy operation info.</returns>
        public CopyOperationInfo CopyTo(string srcPath, string destPath, bool isDeepCopy)
        {
            CopyOperationInfo copyInfo = new() { SrcPath = srcPath, DestPath = destPath};

            _logger.Info($"Start coping {srcPath} -> {destPath}.");
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            uint copiedFilesAmount = CopyDirectory(srcPath, destPath, isDeepCopy);
            copyInfo.CopiedFilesAmount = copiedFilesAmount;
            while (!_threadPool.IsAllTasksComplete()) { }

            stopwatch.Stop();
            copyInfo.CopyTime = stopwatch.Elapsed;
            _logger.Info($"End coping {srcPath} -> {destPath}.");

            return copyInfo;
        }
    }
}
