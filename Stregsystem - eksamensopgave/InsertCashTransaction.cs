using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stregsystem___eksamensopgave
{
    public class InsertCashTransaction : Transaction
    {
        public InsertCashTransaction(User user, DateTime date, decimal amount) : base(user, date, amount)
        {
        }

        public override Transaction Execute()
        {
            User.Deposit(Amount);
            return this;
        }

        public override string ToString()
        {
            return $"Transaction id:{Id} User: {User} deposited {Amount} on the following date {Date.ToString("d") }";
        }
    }
}
