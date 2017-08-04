using Pizzeria.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzeria.ViewModels
{
    public class CurrentOrderViewModel : Order
    {
        public List<ProductViewModel> ProductList { get; set; }
        public string BackgroundColor { get; set; }


        public CurrentOrderViewModel()
        {
            ProductList = new List<ProductViewModel>();
        }

        public CurrentOrderViewModel(Order o)
        {
            ProductList = new List<ProductViewModel>();

            ID = o.ID;
            ClientName = o.ClientName;
            UserEmail = o.UserEmail;
            Phone = o.Phone;
            Date = o.Date;
            Value = o.Value;
            City = o.City;
            Street = o.Street;
            HouseNumber = o.HouseNumber;
            FlatNumber = o.FlatNumber;
            Completed = o.Completed;
        }
    }
}
