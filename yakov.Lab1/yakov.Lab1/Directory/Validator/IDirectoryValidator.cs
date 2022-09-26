using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace yakov.Lab1.Directory.Validator
{
    public interface IDirectoryValidator
    {
        void ValidatePath(string path);
    }
}
