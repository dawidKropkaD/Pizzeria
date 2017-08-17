using Microsoft.AspNetCore.Mvc;
using Pizzeria.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pizzeria.Models;

namespace Pizzeria.ViewComponents
{
    public class BasketViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public BasketViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }


        public Task<IViewComponentResult> InvokeAsync()
        {
            int productNumber = 0;
            var basket = SessionExtensions.Get2(HttpContext.Session, "Basket");

            if (basket != null)
            {
                productNumber = basket.ItemContainerList.Sum(x => x.ItemList.Sum(y => y.Quantity));
            }
                        
            return Task.FromResult<IViewComponentResult>(View("BasketActionLink", productNumber));
        }
    }
}
