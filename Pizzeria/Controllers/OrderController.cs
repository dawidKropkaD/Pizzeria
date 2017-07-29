using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pizzeria.Models;
using Pizzeria.Data;
using Pizzeria.ViewModels;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Pizzeria.Controllers
{
    public class OrderController : Controller
    {
        private readonly Data.ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SelectAdditionalComponents(string productName, string productCategory, decimal price)
        {
            if (productName == null || productCategory == null)
            {
                return NotFound();
            }

            var productDb = _context.ProductDb.SingleOrDefault(x => x.ProductName.Equals(productName) && x.Category.Equals(productCategory) && x.Price == price);
            if (productDb == null)
            {
                return NotFound();
            }

            var availableComponents = _context.AdditionaComponent.Where(x => x.Category.Equals(productCategory)).ToList();
            if (availableComponents.Count() == 0)
            {
                return AddToBasket(productDb.ID, null);
            }

            SelectAdditionalComponentsViewModel additionalComponentsVM = new SelectAdditionalComponentsViewModel();
            additionalComponentsVM.ProductId = productDb.ID;
            additionalComponentsVM.ProductName = productDb.ProductName;
            additionalComponentsVM.ProductComponents = productDb.Components;
            additionalComponentsVM.PriceOfOrder = productDb.Price;

            foreach (var item in availableComponents)
            {
                additionalComponentsVM.AdditionalComponentDetails.Add(new Tuple<int, string, decimal>(item.ID, item.Name, item.Price));
            }


            return View(additionalComponentsVM);
        }

        [HttpPost]
        public IActionResult AddToBasket(int productId, List<int> additionaComponentsIDs)
        {
            Models.SessionExtensions.Add(HttpContext.Session, "Basket", new Tuple<int, List<int>>(productId, additionaComponentsIDs));

            return RedirectToAction("Basket");
        }

        public IActionResult Basket()
        {
            var basket = Models.SessionExtensions.Get<List<Tuple<int, List<int>>>>(HttpContext.Session, "Basket");

            if (basket == null || basket.Count() == 0)
            {
                return View("EmptyBasket");
            }

            BasketViewModel basketVM = new BasketViewModel();

            for (int i = 0; i < basket.Count(); i++)
            {
                var productDb = _context.ProductDb.SingleOrDefault(x => x.ID == basket[i].Item1);
                if (productDb == null)
                    continue;

                var additionalComponent = _context.AdditionaComponent.Where(x => basket[i].Item2.Contains(x.ID)).ToList();
                var additionalComponentName = additionalComponent.Select(x => x.Name).ToList();

                decimal productPrice = productDb.Price;
                foreach (var item in additionalComponent)
                {
                    productPrice += item.Price;
                }

                basketVM.Products.Add(new BasketViewModel.Product(
                    productDb.ProductName, productDb.Components, string.Join(", ", additionalComponentName.ToArray()), productDb.Size, productDb.Weight, productPrice));
            }

            basketVM.OrderPrice = basketVM.Products.Sum(x => x.ProductPrice);

            return View(basketVM);
        }

        [HttpPost]
        public IActionResult DeleteFromBasket(int productIndex)
        {
            Models.SessionExtensions.DeleteProduct(HttpContext.Session, "Basket", productIndex);

            return RedirectToAction("Basket");
        }
    }
}