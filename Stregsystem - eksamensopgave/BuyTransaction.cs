using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stregsystem___eksamensopgave
{
    public class BuyTransaction : Transaction
    {
        private Product Product { get; }

        private int AmountBought {get; }
        
        public BuyTransaction(User user, DateTime date, Product product, int amount) : base(user, date, product.GetPrice())
        {
            Product = product;
            AmountBought = amount;
        }

        public BuyTransaction(User user, DateTime date, Product product) : base(user, date, product.GetPrice())
        {
            Product = product;
            AmountBought = 1;
        }

        public override Transaction Execute()
        {
            if (!Product.IsActive()) throw new ProductNotActiveException(Product);
            else
            {
                User.Buy(Product, AmountBought);
                return this;
            }
            
        }

        public override string ToString()
        {
            return $"Transaction id:{Id} User: \"{User}\" bought {AmountBought}: \"{Product}\" for {Amount}dkk each on the following date {Date.ToString("d") }";
        }
        public Product GetProduct()
        {
            return Product;
        }
    }
}
