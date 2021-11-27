using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stregsystem___eksamensopgave
{
    public abstract class Transaction
    {
        private static int AmountOfTransactions;
        protected int Id {get;}

        protected User _user;

        protected User User
        {
            get { return _user; }
            set { if (value == null) throw new Exception("A transaction has to have a user");
                _user = value; }
        }

        protected DateTime Date { get; }

        protected Decimal Amount { get; }

        public Transaction(User user, DateTime date, Decimal amount)
        {
            User = user;
            Date = date;
            Amount = amount;
            Id = ++AmountOfTransactions;            
        }        
        
        abstract public Transaction Execute();

        public User GetUser() => User;
        public DateTime GetDate() => Date;
    }
}
