using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzeria.Models
{
    public class Product
    {
        public string ProductName { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string Components { get; set; }
        public List<decimal> Price { get; set; }

        /// <summary>
        /// In centimeters
        /// </summary>
        public List<double> Size { get; set; }

        /// <summary>
        /// In gram
        /// </summary>
        public double? Weight { get; set; }

        public bool IsInLocal { get; set; }
        public bool IsOnline { get; set; }

        public Product()
        {
            Price = new List<decimal>();
            Size = new List<double>();
        }
    }
}
