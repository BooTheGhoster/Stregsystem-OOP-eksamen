using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stregsystem___eksamensopgave
{
    class UserDoesNotExistException : Exception
    {
        public UserDoesNotExistException(string username) : base($"A user with the username:'{username}' does not exist.")
        {

        }
    }
}
