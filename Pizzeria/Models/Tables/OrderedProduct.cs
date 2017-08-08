using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzeria.Models.Tables
{
    public class OrderedProduct
    {
        public int ID { get; set; }
        public int OrderId { get; set; }
        public string Name { get; set; }
        public string Components { get; set; }
        public string AdditionalComponents { get; set; }
        public double? Size { get; set; }
        public double? Weight { get; set; }
        public decimal FinalValue { get; set; }
        public int Quantity { get; set; }
    }
}
