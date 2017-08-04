using Pizzeria.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
            [Display(Name = "Produkt")]
            public string ProductName { get; set; }

            public string ProductComponents { get; set; }

            [Display(Name = "Dodatkowe składniki")]
            public string AdditionalComponents { get; set; }

            [Display(Name = "Rozmiar")]
            [DisplayFormat(DataFormatString = "{0:###.##}cm")]
            public double? Size { get; set; }

            public double? Weight { get; set; }

            [Display(Name = "Cena")]
            [DisplayFormat(DataFormatString = "{0:n2} zł")]
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
