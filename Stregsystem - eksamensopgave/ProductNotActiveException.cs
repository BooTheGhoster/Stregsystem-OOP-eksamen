using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stregsystem___eksamensopgave
{
    class ProductNotActiveException : Exception
    {
        public ProductNotActiveException(Product product) : base($"Product: {product} is not active")
        {

        }
    }
}
