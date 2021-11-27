using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Stregsystem___eksamensopgave
{
    class Stregsystem : IStregsystem
    {
        private List<Transaction> Transactions { get; }
        private List<User> Users { get; }
        private List<Product> Products { get; }

        public List<Product> GetActiveProducts()
        {            
            return Products.FindAll(product => product.IsActive());            
        }
        

        private TransactionLogger Logger;

        
        public Stregsystem()
        {
            Logger = new();
            Users = new();
            Products = new();
            Transactions = new();
            String productFilePath = System.AppDomain.CurrentDomain.BaseDirectory.Replace("\\Stregsystem - eksamensopgave\\Stregsystem - eksamensopgave\\bin\\Debug\\net5.0\\", "\\products.csv");
            String usersFilePath = System.AppDomain.CurrentDomain.BaseDirectory.Replace("\\Stregsystem - eksamensopgave\\Stregsystem - eksamensopgave\\bin\\Debug\\net5.0\\", "\\users.csv");
            try
            {
                LoadProducts(productFilePath);
                LoadUsers(usersFilePath);
            }
            catch
            {
                throw new Exception("Either the userfile or productfile is in use.");
            }
        }
                
        public BuyTransaction BuyProduct(User user, Product product)
        {
            Transaction transaction = new BuyTransaction(user, DateTime.Now, product);
            return (BuyTransaction)ExecuteTransaction(transaction);
        }
        public BuyTransaction BuyProduct(User user, Product product, int amount)
        {
            Transaction transaction = new BuyTransaction(user, DateTime.Now, product, amount);
            return (BuyTransaction)ExecuteTransaction(transaction);
        }

        public InsertCashTransaction AddCreditsToAccount(User user, Decimal amount)
        {
            Transaction transaction = new InsertCashTransaction(user, DateTime.Now, amount);
            return (InsertCashTransaction)ExecuteTransaction(transaction);
        }

        public Transaction ExecuteTransaction(Transaction transaction)
        {
            transaction.Execute(); // we throw an exception if something goes wrong                
            Logger.LogTransactionAsync(transaction);
            Transactions.Add(transaction);
            return transaction;                                           
        }

        public Product GetProductByID(int id)
        {
            try
            {
                int index = Products.FindIndex(product => product.GetId() == id);
                return Products[index];
            }
            catch
            {
                throw new ProductDoesNotExistException(id);
            }
        }

        public IEnumerable<User> GetUsers(Func<User, bool> predicate)
        {
            List<User> returnList = new();
            foreach (User user in Users)
            {
                if (predicate(user))
                {
                    returnList.Add(user);
                }
            }
            return returnList;
        }
        public event UserBalanceNotification UserBalanceWarning;

        public void BalanceIsLow(User user, Decimal amount)
        {
            UserBalanceWarning.Invoke(user, amount);   
        }

        public User GetUserByUsername(string username)
        {
            if (Users.Count == 0) throw new UserDoesNotExistException(username);
            int index = Users.FindIndex(user => user.GetUsername() == username);
            if (index == -1) throw new UserDoesNotExistException(username);
            else return Users[index];
        }

        public IEnumerable<Transaction> GetTransactions(User user, int amount)
        {
            List<Transaction> returnList = new();
            List<Transaction> userTransactionList = Transactions.FindAll(transaction => transaction.GetUser() == user);
            userTransactionList.Sort();
            for (int i = 0; i < amount && i <userTransactionList.Count; i++)
            {
                returnList.Add(userTransactionList[i]);
            }
            return returnList;
        }
        public List<Product> ActiveProducts()
        {
            return Products.FindAll(product => product.IsActive());
        }

        public void LoadUsers(string filepath)
        {
            Char[] splitArray = new char[] { ',', ';', ':' };
            if (!File.Exists(filepath)) return;
            List<String> linesListString = File.ReadLines(filepath).ToList();
            List<String[]> linesList = linesListString.Select(line => line.Split(splitArray)).ToList();
            
            int idIndex = Array.FindIndex(linesList[0], x => x == "id");
            int firstnameIndex = Array.FindIndex(linesList[0], x => x == "firstname");
            int lastnameIndex = Array.FindIndex(linesList[0], x => x == "lastname");
            int usernameIndex = Array.FindIndex(linesList[0], x => x == "username");
            int balanceIndex = Array.FindIndex(linesList[0], x => x == "balance");
            int emailIndex = Array.FindIndex(linesList[0], x => x == "email");

            for (int i = 1; i < linesList.Count; i++)
            {
                try
                {
                    String[] line = linesList[i];
                    User user = new(line[firstnameIndex], line[lastnameIndex], line[usernameIndex], line[emailIndex], Convert.ToDecimal(line[balanceIndex]), Convert.ToInt32(line[idIndex]));
                    AddUser(user);                    
                }
                catch
                {
                    throw new FileErrorException(filepath);
                }
            }

        }

        public void AddUser(User user)
        {
            if (Users.FindIndex(lambdaUser => lambdaUser.GetId() == user.GetId()) != -1) throw new Exception("Duplicate user id");
            else
            {
                Users.Add(user);
                user.LowBalance += BalanceIsLow;
            }
        }
        public void LoadProducts(string filepath)
        {
            Char[] splitArray = new char[] { ',', ';'};
            List<String[]> linesList = File.ReadLines(filepath).ToList().Select(line => line.Split(splitArray)).ToList();

            int idIndex = Array.FindIndex(linesList[0], x => x == "id");
            int nameIndex = Array.FindIndex(linesList[0], x => x == "name");
            int priceIndex = Array.FindIndex(linesList[0], x => x == "price");
            int activeIndex = Array.FindIndex(linesList[0], x => x == "active");

            // We fix eventual special characters in the product name
            for (int i = 0; i <  linesList.Count; i++)
            {
                String[] arr = linesList[i];
                if (arr.Length > 5)
                {
                    string newName = "";
                    for (int j = nameIndex; j < arr.Length - 3; j++)
                    {
                        newName += arr[j];
                    }
                    String[] newArr = new string[4];
                    newArr[idIndex] = arr[idIndex < nameIndex ? idIndex : arr.Length - (5 - idIndex)];
                    newArr[priceIndex] = arr[priceIndex < nameIndex ? priceIndex : arr.Length - (5 - priceIndex)];
                    newArr[activeIndex] = arr[activeIndex < nameIndex ? activeIndex : arr.Length - (5 - activeIndex)];
                    newArr[nameIndex] = newName;
                    linesList[i] = newArr;
                }
                // We remove html tags
                linesList[i][nameIndex] = Regex.Replace(linesList[i][nameIndex], "<.+?>", "").Trim('"');                
            }

            

            for (int i = 1; i < linesList.Count; i++)
            {
                try
                {                    
                    String[] line = linesList[i];
                    bool boole = Convert.ToBoolean(Convert.ToInt32(line[activeIndex]));
                    Product product = new(line[nameIndex].ToString(), Convert.ToDecimal(line[priceIndex]), boole, Convert.ToInt32(line[idIndex]));
                    AddProduct(product);
                }
                catch
                {
                    throw new FileErrorException(filepath);
                }
            }
        }

        public void AddProduct(Product product)
        {
            if (Products.FindIndex(lambdaProduct => lambdaProduct.GetId() == product.GetId()) != -1) throw new Exception("Duplicate product id");
            else Products.Add(product);
        }


    }
}
