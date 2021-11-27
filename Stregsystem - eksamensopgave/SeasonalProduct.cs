using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stregsystem___eksamensopgave
{
    public class SeasonalProduct : Product
    {
        private DateTime _seasonalStartDate;
        private DateTime SeasonalStartDate { get
            {
                return _seasonalStartDate;
            }
            set
            {
                DateTime newValue = new DateTime(1, value.Month, value.Day);
                if (SeasonalEndDate.Year != 1)
                {
                    Console.WriteLine("It works");                    
                    _seasonalStartDate = newValue;
                }
                else
                {
                    if (SeasonalEndDate < newValue) throw new Exception("SeasonalEndDate must be later than SeasonalStartDate");
                    else _seasonalStartDate = newValue;
                }
            }
        }

        private DateTime _seasonalEndDate;
        private DateTime SeasonalEndDate
        {
            get
            {
                return _seasonalEndDate;
            }
            set
            {
                DateTime newValue = new DateTime(1, value.Month, value.Day);
                if (SeasonalStartDate.Year != 1)
                {
                    Console.WriteLine("It works");
                    _seasonalEndDate = newValue;
                }
                else
                {
                    if (SeasonalStartDate < newValue) throw new Exception("SeasonalStartDate must be later than SeasonalEndDate");
                    else _seasonalEndDate = newValue;
                }
            }
        }
        public SeasonalProduct(string name, Decimal price, DateTime seasonalStartDate, DateTime seasonalEndDate) : base(name, price)
        {
            SeasonalEndDate = seasonalEndDate;
            SeasonalStartDate = seasonalStartDate;
        }
    }
}
