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
        public List<ItemsContainer> ItemsContainerList { get; set; }
        public decimal OrderValue { get; set; }


        public BasketViewModel()
        {
            ItemsContainerList = new List<ItemsContainer>();
        }


        public class ItemsContainer
        {
            public List<ProductViewModel> ProductVMList { get; set; }
            public string PromotionShortDescription { get; set; }

            public ItemsContainer()
            {
                ProductVMList = new List<ProductViewModel>();
            }
        }
    }
}
