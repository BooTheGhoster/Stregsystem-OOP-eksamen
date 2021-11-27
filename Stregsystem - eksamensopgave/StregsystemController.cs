using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stregsystem___eksamensopgave
{
    class StregsystemController
    {
        StregsystemCommandParser parser;
        public StregsystemController(IStregsystem stregsystem, IStregsystemUI stregsystemUI)
        {
            parser = new(stregsystemUI, stregsystem);
            stregsystemUI.CommandEntered += OnCommandEntered;            
        }

        private void OnCommandEntered(string command)
        {            
            parser.ParseCommand(command);
        }
    }
}
