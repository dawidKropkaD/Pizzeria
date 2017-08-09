using Pizzeria.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzeria.ViewModels
{
    public class CurrentOrdersViewModel 
    {
        public List<CurrentOrder> CurrentOrderList { get; set; }
        public bool Sound { get; set; }


        public CurrentOrdersViewModel()
        {
            CurrentOrderList = new List<CurrentOrder>();
        }


        public class CurrentOrder : Order
        {
            public List<ProductViewModel> ProductList { get; set; }
            public string BackgroundColor { get; set; }
            public string HtmlClass { get; set; }


            public CurrentOrder()
            {
                ProductList = new List<ProductViewModel>();
            }

            public CurrentOrder(Order o)
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
}
