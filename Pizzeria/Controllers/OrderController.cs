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
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using Pizzeria.Models.Tables;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace Pizzeria.Controllers
{
    public class OrderController : Controller
    {
        private readonly Data.ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrderController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
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

            OrderBusinessLayer orderBL = new OrderBusinessLayer();
            BasketViewModel basketVM = new BasketViewModel();

            for (int i = 0; i < basket.Count(); i++)
            {
                var productDb = _context.ProductDb.SingleOrDefault(x => x.ID == basket[i].Item1);
                if (productDb == null)
                    continue;

                var additionalComponent = _context.AdditionaComponent.Where(x => basket[i].Item2.Contains(x.ID)).ToList();

                decimal productPrice = productDb.Price + orderBL.GetAdditionalComponentsPrice(additionalComponent);

                basketVM.Products.Add(new ProductViewModel(
                    productDb.ProductName, productDb.Components, orderBL.GetAdditionalComponentsName(additionalComponent), productDb.Size, productDb.Weight, productPrice));
            }

            basketVM.OrderPrice = orderBL.GetOrderValue(
                basketVM.Products.Select(x => x.ProductPrice).Sum(),
                User.IsInRole("Member"),
                _userManager.GetUserId(User),
                _context
            );

            return View(basketVM);
        }

        [HttpPost]
        public IActionResult DeleteFromBasket(int productIndex)
        {
            Models.SessionExtensions.DeleteProduct(HttpContext.Session, "Basket", productIndex);

            return RedirectToAction("Basket");
        }

        public async Task<IActionResult> DeliveryForm()
        {
            var basket = Models.SessionExtensions.Get<List<Tuple<int, List<int>>>>(HttpContext.Session, "Basket");
            if (basket == null || basket.Count() == 0)
            {
                return View("EmptyBasket");
            }

            DeliveryFormViewModel deliveryFormVM = new DeliveryFormViewModel();
            deliveryFormVM.City = "Warszawa";

            if (User.IsInRole("Employee") || User.IsInRole("Admin"))
            {
                return View("DeliveryFormForStaff", deliveryFormVM);
            }
            
            if (User.IsInRole("Member"))
            {
                deliveryFormVM.DisplayForMember = "none";

                var user = await GetCurrentUserAsync();

                deliveryFormVM.ClientName = user.UserName;
                deliveryFormVM.Email = user.Email;
                //deliveryFormVM.Phone = Int32.Parse(user.PhoneNumber);

                var lastOrder = _context.Order.LastOrDefault(x => x.UserEmail.Equals(user.Email));
                if (lastOrder != null)
                {
                    deliveryFormVM.City = lastOrder.City;
                    deliveryFormVM.Street = lastOrder.Street;
                    deliveryFormVM.HouseNumber = lastOrder.HouseNumber;
                    deliveryFormVM.FlatNumber = lastOrder.FlatNumber;
                    deliveryFormVM.Phone = lastOrder.Phone;
                }
            }

            return View(deliveryFormVM);
        }

        [HttpPost]
        public async Task<IActionResult> DeliveryForm([Bind("City, Street, HouseNumber, FlatNumber, ClientName, Email, Phone, DisplayForMember")] DeliveryFormViewModel deliveryFormVM)
        {
            if (User.Identity.IsAuthenticated == false && String.IsNullOrEmpty(deliveryFormVM.Email))
            {
                ModelState.AddModelError("Email", "Adres email jest wymagany");
            }

            if (ModelState.IsValid)
            {
                var basket = Models.SessionExtensions.Get<List<Tuple<int, List<int>>>>(HttpContext.Session, "Basket");
                if (basket == null || basket.Count() == 0)
                {
                    return View("EmptyBasket");
                }

                OrderBusinessLayer orderBL = new OrderBusinessLayer();
                List<OrderedProduct> orderedProductList = orderBL.GetOrderedProductList(basket, _context);
                
                //Create order
                Order order = new Order();
                var user = await GetCurrentUserAsync();

                if (User.IsInRole("Member"))
                    order.ClientName = user.UserName;
                else
                    order.ClientName = deliveryFormVM.ClientName;

                if (User.Identity.IsAuthenticated)
                    order.UserEmail = user.Email;
                else
                    order.UserEmail = deliveryFormVM.Email;

                order.Phone = deliveryFormVM.Phone;
                order.Date = DateTime.Now;
                order.Value = orderBL.GetOrderValue(orderedProductList.Sum(x => x.Value), User.IsInRole("Member"),
                    _userManager.GetUserId(User), _context, true);
                order.City = deliveryFormVM.City;
                order.Street = deliveryFormVM.Street;
                order.HouseNumber = deliveryFormVM.HouseNumber;
                order.FlatNumber = deliveryFormVM.FlatNumber;

                _context.Add(order);
                await _context.SaveChangesAsync();

                int orderId = order.ID;
                int loyaltyPoints = (int)Math.Floor((double)order.Value / 20.00);
                orderBL.AddLoyaltyPoints(loyaltyPoints, User.IsInRole("Member"), _userManager.GetUserId(User), _context);

                for (int i = 0; i < orderedProductList.Count(); i++)
                {
                    orderedProductList[i].OrderId = orderId;
                    _context.Add(orderedProductList[i]);
                }
                await _context.SaveChangesAsync();

                HttpContext.Session.Remove("Basket");

                //Return specific View
                if (User.IsInRole("Employee") || User.IsInRole("Admin"))
                {
                    return RedirectToAction("CurrentOrders");
                }
                return View("OrderSuccess");
            }
            else if(User.IsInRole("Employee") || User.IsInRole("Admin"))
            {
                return View("DeliveryFormForStaff", deliveryFormVM);
            }
            
            return View(deliveryFormVM);
        }


        [Authorize(Roles = "Admin, Employee")]
        public IActionResult CurrentOrders()
        {
            List<CurrentOrderViewModel> currentOrderVMList = new List<CurrentOrderViewModel>();
            List<Order> currentOrderList = _context.Order.Where(x => x.Completed == false).ToList();

            foreach (var item in currentOrderList)
            {
                CurrentOrderViewModel currentOrderVM = new CurrentOrderViewModel(item);
                
                var orderedProductList = _context.OrderedProduct
                    .Where(x => x.OrderId == item.ID)
                    .Select(i => new { i.Name, i.AdditionalComponents, i.Size, i.Value })
                    .ToList();

                foreach (var orderedProduct in orderedProductList)
                {
                    ProductViewModel product = new ProductViewModel();

                    product.ProductName = orderedProduct.Name;
                    product.Size = orderedProduct.Size;
                    product.AdditionalComponents = orderedProduct.AdditionalComponents;
                    product.ProductPrice = orderedProduct.Value;

                    currentOrderVM.ProductList.Add(product);
                }

                if (currentOrderVMList.Count() % 2 == 0)
                    currentOrderVM.BackgroundColor = "lightblue";
                else
                    currentOrderVM.BackgroundColor = "lightcyan";

                currentOrderVMList.Add(currentOrderVM);
            }

            var isAjax = Request.Headers["X-Requested-With"] == "XMLHttpRequest";

            if (isAjax)
            {
                return PartialView("_CurrentOrdersPartial", currentOrderVMList);
            }
            else
            {
                return View(currentOrderVMList);
            }
        }


        [HttpPost]
        public IActionResult OrderCompleted(string[] values)
        {
            if (values != null && values.Length != 0)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    Order order = _context.Order.SingleOrDefault(x => x.ID == Int32.Parse(values[i]));
                    order.Completed = true;

                    try
                    {
                        _context.Update(order);
                        _context.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        throw;
                    }
                }
            }
            return RedirectToAction("CurrentOrders");
        }


        [Authorize]
        public async Task<IActionResult> MyOrders()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            List<MyOrderViewModel> myOrderVMList = new List<MyOrderViewModel>();
            var myOrderDbList = _context.Order
                .Where(x => x.UserEmail.Equals(user.Email))
                .Select(x => new { x.ID, x.Value, x.Date })
                .ToList();

            if (myOrderDbList == null || myOrderDbList.Count() == 0)
            {
                return View("NoOrders");
            }

            for (int i = 0; i < myOrderDbList.Count(); i++)
            {
                MyOrderViewModel myOrderVM = new MyOrderViewModel();
                var orderedProductList = _context.OrderedProduct
                    .Where(x => x.OrderId == myOrderDbList[i].ID)
                    .Select(x => new { x.Name, x.Components, x.AdditionalComponents, x.Size, x.Weight, x.Value })
                    .ToList();

                for (int j = 0; j < orderedProductList.Count(); j++)
                {
                    ProductViewModel product = new ProductViewModel(
                        orderedProductList[j].Name,
                        orderedProductList[j].Components,
                        orderedProductList[j].AdditionalComponents,
                        orderedProductList[j].Size,
                        orderedProductList[j].Weight,
                        orderedProductList[j].Value
                    );

                    myOrderVM.ProductList.Add(product);
                }

                myOrderVM.Value = myOrderDbList[i].Value;
                myOrderVM.OrderDate = myOrderDbList[i].Date;
                myOrderVMList.Add(myOrderVM);
            }

            return View(myOrderVMList);
        }


        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);
    }
}