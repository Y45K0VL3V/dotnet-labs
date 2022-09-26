using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace yakov.Lab1.Directory.Validator
{
    public class DirectoryValidator : IDirectoryValidator
    {
        public void ValidatePath(string path)
        {
            if (!System.IO.Directory.Exists(path))
                throw new Exception($"Directory {path} - not exist."); 
        }
    }
}
