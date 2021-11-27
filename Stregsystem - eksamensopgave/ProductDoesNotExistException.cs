using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stregsystem___eksamensopgave
{
    class ProductDoesNotExistException : Exception
    {
        public ProductDoesNotExistException(int id) : base($"A product with product id {id} does not exist")
        {

        }

    }
}
