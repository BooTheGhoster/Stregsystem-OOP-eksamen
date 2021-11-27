using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stregsystem___eksamensopgave
{
    class InsufficientCreditsException : Exception
    {
        public InsufficientCreditsException(User user, Product product) : base($"User: {user} tried to buy {product} with insufficient funds")
        {
            
        }
    }
}
