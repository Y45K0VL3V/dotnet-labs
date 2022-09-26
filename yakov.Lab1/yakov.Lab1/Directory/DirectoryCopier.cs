using yakov.ThreadPool;
using System.IO;
using yakov.Lab1.Directory.Validator;

namespace yakov.Lab1.Directory
{
    public class DirectoryCopier
    {
        public DirectoryCopier(IThreadPool threadPool)
        {
            _threadPool = threadPool;
        }

        private IThreadPool _threadPool;

        public void CopyTo(string srcPath, string destPath, IDirectoryValidator directoryValidator)
        {
            try
            {
                directoryValidator.ValidatePath(srcPath);
                directoryValidator.ValidatePath(destPath);
            }
            catch
            {
                throw;
            }

        }
    }
}
