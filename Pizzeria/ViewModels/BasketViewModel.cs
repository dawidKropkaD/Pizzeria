using Pizzeria.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzeria.ViewModels
{
    public class BasketViewModel
    {
        public List<Product> Products { get; set; }
        public decimal OrderPrice { get; set; }


        public BasketViewModel()
        {
            Products = new List<Product>();
        }


        public class Product
        {
            public string ProductName { get; set; }
            public string ProductComponents { get; set; }
            public string AdditionalComponents { get; set; }
            public double? Size { get; set; }
            public double? Weight { get; set; }
            public decimal ProductPrice { get; set; }


            public Product()
            {
            }

            public Product(string name, string components, string additionalComponents, double? size, double? weight, decimal price)
            {
                ProductName = name;
                ProductComponents = components;
                AdditionalComponents = additionalComponents;
                Size = size;
                Weight = weight;
                ProductPrice = price;
            }
        }

    }
}
