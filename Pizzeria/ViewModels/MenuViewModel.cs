using Pizzeria.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzeria.ViewModels
{
    public class MenuViewModel
    {
        public List<Product> Menu { get; set; }
        public List<string> Categories { get; set; }
        public List<string> CategoriesId { get; set; }
        public List<string> UserRoles { get; set; }
        public bool CanOrderProduct { get; set; }


        public MenuViewModel()
        {
        }

        public MenuViewModel(List<Product> menu, List<string> userRoles, bool canOrderProduct)
        {
            Menu = menu;
            UserRoles = userRoles;
            CanOrderProduct = canOrderProduct;
        }
    }
}
