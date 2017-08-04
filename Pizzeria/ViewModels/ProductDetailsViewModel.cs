using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzeria.ViewModels
{
    public class ProductDetailsViewModel
    {
        [Display(Name = "Składniki")]
        public string Components { get; set; }

        [Display(Name = "Dodatkowe składniki")]
        public string AdditionalComponents { get; set; }

        [DisplayFormat(DataFormatString = "{0:###.##}g")]
        public double? Weight { get; set; }



        public ProductDetailsViewModel()
        {
        }

        public ProductDetailsViewModel(string components, string additionalComponents, double? weight)
        {
            Components = components;
            AdditionalComponents = additionalComponents;
            Weight = weight;
        }
    }
}
