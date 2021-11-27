using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stregsystem___eksamensopgave
{
    public delegate void UserBalanceNotification(User user, decimal balance);

    public interface IStregsystem
    {
        public List<Product> GetActiveProducts();
        public InsertCashTransaction AddCreditsToAccount(User user, Decimal amount); 
        public BuyTransaction BuyProduct(User user, Product product);
        public BuyTransaction BuyProduct(User user, Product product, int amount);
        public Product GetProductByID(int id);
        public IEnumerable<Transaction> GetTransactions(User user, int count); 
        public IEnumerable<User> GetUsers(Func<User, bool> predicate); 
        public User GetUserByUsername(string username); 

        public event UserBalanceNotification UserBalanceWarning;        

        
    }
}
