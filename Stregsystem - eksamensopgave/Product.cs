using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stregsystem___eksamensopgave
{
    public class Product
    {
        static protected int AmountOfProducts;
        protected int Id { get; }

        protected string _name;
        protected string Name { get
            {
                return _name;
            } set
            {
                if (value != null) _name = value;
            }
        }

        protected Decimal _price;

        protected Decimal Price
        {
            get { return _price; }
            set { if (value > 0) _price = value; }
        }
        protected bool Active { get; 
            set; }

        public bool CanBeBoughtOnCredit { get; protected set; }


        public Product(string name, Decimal price)
        {
            Name = name;
            Price = price;
            Active = true;
            CanBeBoughtOnCredit = false;
            Id = ++AmountOfProducts;
        }

        public Product(string name, Decimal price, bool active, int id)
        {
            Name = name;
            Price = price;
            Active = active;
            CanBeBoughtOnCredit = false;
            Id = id;
            AmountOfProducts = ++id;
        }

        public void SetCanBeBoughtOnCredit(bool boolean)
        {
            CanBeBoughtOnCredit = boolean;
        }
        public void SetActive(bool boolean)
        {
            Active = boolean;
        }
        public override string ToString()
        {
            return $"{Id} {Name.ToString()} {Price}";
        }

        public string GetName()
        {
            return Name;
        }
        
       public bool IsActive()
        {
            return Active;
        }
        public Decimal GetPrice()
        {
            return Price;
        }        
        public int GetId()
        {
            return Id;
        }
    }
}
