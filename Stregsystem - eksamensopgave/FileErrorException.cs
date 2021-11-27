using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stregsystem___eksamensopgave
{
    class FileErrorException : Exception
    {
        public FileErrorException(string filepath) : base($"The file at {filepath} is wrongly formatted or could not be opened")
        {

        }
    }
}
