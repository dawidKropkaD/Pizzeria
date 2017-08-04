using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzeria.ViewModels
{
    public class MyOrderViewModel
    {
        [Display(Name = "Zamówienie")]
        public List<BasketViewModel.Product> ProductList { get; set; }

        [Display(Name = "Wartość")]
        [DisplayFormat(DataFormatString = "{0:n2} zł")]
        public decimal Value { get; set; }

        [Display(Name = "Data")]
        [DisplayFormat(DataFormatString = "{0:d/MM/yyyy H:mm}")]
        public DateTime OrderDate { get; set; }

        public MyOrderViewModel()
        {
            ProductList = new List<BasketViewModel.Product>();
        }
    }
}
