using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Stregsystem___eksamensopgave
{
    public class User : IComparable
    {
        static int AmountOfUsers;
        private int ID { get;}
        private string _username;
        private string Username { get
            {
                return _username;
            }
            set
            {                
                Regex usernameChecker = new Regex("[a-z0-9_]");
                if (usernameChecker.IsMatch(value)) _username = value;                
                else throw new Exception("Wrong username format");               
            }
        }

        private string _firstname;
        private string Firstname
        {
            get
            {
                return _firstname;
            }
            set
            {
                if (value == null) throw new Exception("First name cannot be null");
                Regex firstnameChecker = new Regex("[A-Za-z']*");
                if (!firstnameChecker.IsMatch(value)) throw new Exception("First name can only have english letters and apostrophes");
                _firstname = value;
            }
        }

        private string _lastname;
        private string Lastname
        {
            get
            {
                return _lastname;
            }
            set
            {
                if (value == null) throw new Exception("Lastname cannot be null");
                Regex lastnameChecker = new Regex("[A-Za-z']*");
                if (!lastnameChecker.IsMatch(value)) throw new Exception("Last name can only have english letters and apostrophes");
                _lastname = value;
            }
        }

        private string _email;
        private string Email { get
            {
                return _email;
            }
            set
            {
                Regex emailChecker = new Regex("[a-zA-Z0-9._-]+[@][a-zA-Z0-9][a-zA-Z0-9]*[._]+[a-zA-Z0-9.-]*");
                
                if (emailChecker.IsMatch(value) && 
                    (value[value.Length - 1] != '.' && 
                    value[value.Length - 1] != '-') &&
                    !value.Any(character => character =='+')) 
                    _email = value;
                else throw new Exception("Email address has wrong format");
            }
        }
        private Decimal _balance;
        private Decimal Balance { get
            {
                return _balance;
            }
            set
            {
                _balance = value;
                if (value > 0 && _balance < 5000 && ID != 0)
                {
                    if (this != null) LowBalance.Invoke(this, Balance);
                    
                }
            }
        }
        public delegate void UserBalanceNotification(User user, decimal balance);
        public event UserBalanceNotification LowBalance;
        public User(string firstname, string lastname, string username, string email, decimal balance, int id)
        {
            Email = email;
            Firstname = firstname;
            Lastname = lastname;
            Username = username;
            Balance = balance;
            ID = id;
            AmountOfUsers = ++id;
        }
        public override string ToString()
        {           
            return $"{Firstname} {Lastname} {Email}";
        }
        public int CompareTo(Object obj)
        {
            if (obj == null) throw new Exception("Cannot compare null to a user");
            if (obj is not User) throw new Exception("Cannot compare User to something that is not a user");
            return ID - ((User)obj).ID;            
        }

        public override bool Equals(object obj)
        {
            return obj is User user &&
                   ID == user.ID &&
                   Username == user.Username &&
                   Firstname == user.Firstname &&
                   Lastname == user.Lastname &&
                   Email == user.Email &&
                   Balance == user.Balance;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ID, Username, Firstname, Lastname, Email, Balance);
        }

        public bool Buy(Product product, int amount)
        {
            if (product.GetPrice() * amount > Balance && !product.CanBeBoughtOnCredit)
            {
                throw new InsufficientCreditsException(this, product);
            }
            else
            {
                Balance = Decimal.Add(-product.GetPrice() * amount, Balance);
                return true;
            }
        }

        public bool Deposit(Decimal amount)
        {
            Balance += amount;
            return true;
        }

        public string GetUsername()
        {
            return Username;
        }

        public int GetId()
        {
            return ID;
        }
        
        public Decimal GetBalance()
        {
            return Balance;
        }
    }
}
