using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzeria.Models.Tables
{
    public class UserDb : Address
    {
        public int ID { get; set; }
        public string AspNetUserId { get; set; }
        public int LoyaltyPoints { get; set; }

        /// <summary>
        /// Money for loyalty points 
        /// </summary>
        public decimal MoneyPrize { get; set; }
    }
}
