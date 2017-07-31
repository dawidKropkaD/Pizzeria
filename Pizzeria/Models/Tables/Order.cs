using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzeria.Models.Tables
{
    public class Order
    {
        public int ID { get; set; }
        public string ClientName { get; set; }
        public string UserEmail { get; set; }
        public int Phone { get; set; }
        public DateTime Date { get; set; }
        public decimal Value { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public string FlatNumber { get; set; }
    }
}
