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
        public List<ProductViewModel> Products { get; set; }
        public decimal OrderPrice { get; set; }


        public BasketViewModel()
        {
            Products = new List<ProductViewModel>();
        }
    }
}
