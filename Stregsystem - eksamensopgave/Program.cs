using System;
using System.Collections.Generic;

namespace Stregsystem___eksamensopgave
{
    class Program
    {
        static void Main(string[] args)
        {            
            IStregsystem stregsystem = new Stregsystem();
            IStregsystemUI ui = new StregsystemCLI(stregsystem);
            StregsystemController sc = new(stregsystem, ui);
            ui.Start();

        }
    }
}

