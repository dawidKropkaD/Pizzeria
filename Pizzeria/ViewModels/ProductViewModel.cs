using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzeria.ViewModels
{
    public class ProductViewModel : ProductDetailsViewModel
    {
        [Display(Name = "Produkt")]
        public string ProductName { get; set; }

        [Display(Name = "Rozmiar")]
        [DisplayFormat(DataFormatString = "{0:###.##}cm")]
        public double? Size { get; set; }

        [Display(Name = "Wartość")]
        [DisplayFormat(DataFormatString = "{0:n2} zł")]
        public decimal FinalValue { get; set; }

        public decimal ProductValue { get; set; }

        public decimal AdditionalComponentsValue { get; set; }

        [Display(Name = "Ilość")]
        public int Quantity { get; set; }


        public ProductViewModel()
        {
        }

        public ProductViewModel(string name, string components, string additionalComponents, double? size, 
            double? weight, decimal finalValue, int quantity, decimal productValue = 0, decimal additionalComponentsValue = 0)
        {
            ProductName = name;
            Components = components;
            AdditionalComponents = additionalComponents;
            Size = size;
            Weight = weight;
            FinalValue = finalValue;
            Quantity = quantity;
            ProductValue = productValue;
            AdditionalComponentsValue = additionalComponentsValue;
        }
    }
}
