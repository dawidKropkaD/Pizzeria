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

        public IActionResult SelectPromotion(string productName, string productCategory, decimal price)
        {
            OrderBusinessLayer orderBL = new OrderBusinessLayer();
            var productDb = orderBL.TryGetOnlineProduct(_context, productName, productCategory, price);

            if (productDb == null)
            {
                return View("Error");
            }

            var promotionDbList = _context.Promotion.Where(x => x.ProductCategory.Equals(productCategory)).ToList();

            if (promotionDbList == null || promotionDbList.Count() == 0)
            {
                return RedirectToAction("SelectAdditionalComponents", new { productId = productDb.ID });
            }

            SelectPromotionViewModel selectPromoionVM = new SelectPromotionViewModel();
            selectPromoionVM.ProductId = productDb.ID;

            for (int i = 0; i < promotionDbList.Count(); i++)
            {
                selectPromoionVM.PossiblePromotionList.Add(new SelectPromotionViewModel.SelectPromotion(
                    promotionDbList[i].ID, promotionDbList[i].Description, promotionDbList[i].ShortDescription));
            }

            return View(selectPromoionVM);
        }

        public IActionResult SelectAdditionalComponents(int productId, int? selectedPromotionId = null)
        {
            OrderBusinessLayer orderBL = new OrderBusinessLayer();
            var productDb = orderBL.TryGetOnlineProduct(_context, productId);

            if (productDb == null)
            {
                return View("Error");
            }

            var availableComponents = _context.AdditionaComponent.Where(x => x.Category.Equals(productDb.Category)).ToList();
            if (availableComponents.Count() == 0)
            {
                return AddToBasket(productDb.ID, null, selectedPromotionId);
            }

            SelectAdditionalComponentsViewModel additionalComponentsVM = new SelectAdditionalComponentsViewModel();
            additionalComponentsVM.ProductId = productDb.ID;
            additionalComponentsVM.ProductName = productDb.ProductName;
            additionalComponentsVM.ProductComponents = productDb.Components;
            additionalComponentsVM.PriceOfOrder = productDb.Price;
            additionalComponentsVM.SelectedPromotionId = selectedPromotionId;

            foreach (var item in availableComponents)
            {
                additionalComponentsVM.AdditionalComponentDetails.Add(new Tuple<int, string, decimal>(item.ID, item.Name, item.Price));
            }
            
            return View(additionalComponentsVM);
        }

        [HttpPost]
        public IActionResult AddToBasket(int productId, List<int> additionalComponentsIDs, int? selectedPromotionId = null)
        {
            OrderBusinessLayer orderBL = new OrderBusinessLayer();
            var productDb = orderBL.TryGetOnlineProduct(_context, productId);

            if (productDb == null)
            {
                return View("Error");
            }

            if(orderBL.SelectedComponentsValid(_context, additionalComponentsIDs, productDb.Category) == false)
            {
                return View("Error");
            }

            int? promotionId = null;
            if(orderBL.SelectedPromotionIdValid(_context, selectedPromotionId, productDb.Category))
            {
                promotionId = selectedPromotionId;
            }

            Models.SessionExtensions.AddContainer(HttpContext.Session, productId, additionalComponentsIDs, promotionId);

            return RedirectToAction("Basket");
        }

        public IActionResult Basket()
        {
            var basket = Models.SessionExtensions.Get2(HttpContext.Session, "Basket");

            if (basket == null || basket.ItemContainerList.Count() == 0)
            {
                return View("EmptyBasket");
            }

            OrderBusinessLayer orderBL = new OrderBusinessLayer();
            BasketViewModel basketVM = new BasketViewModel();
            decimal baseOrderPrice = 0;

            for (int i = 0; i < basket.ItemContainerList.Count(); i++)
            {
                BasketViewModel.ItemsContainer itemsContainer = new BasketViewModel.ItemsContainer();
                int? promotionId = basket.ItemContainerList[i].PromotionId;

                itemsContainer.PromotionShortDescription = "";
                if (promotionId != null)
                    itemsContainer.PromotionShortDescription = _context.Promotion.Single(x => x.ID == (int)promotionId).ShortDescription;

                for (int j = 0; j < basket.ItemContainerList[i].ItemList.Count(); j++)
                {
                    ProductDb productDb = _context.ProductDb.Single(x => x.ID == basket.ItemContainerList[i].ItemList[j].ProductId);
                    List<AdditionalComponent> additionalComponentDbList = _context.AdditionaComponent
                        .Where(x => basket.ItemContainerList[i].ItemList[j].AdditionalComponentIdList.Contains(x.ID))
                        .ToList();

                    itemsContainer.ProductVMList.Add(new ProductViewModel(productDb.ProductName, productDb.Components,
                        orderBL.GetAdditionalComponentsName(additionalComponentDbList), productDb.Size,
                        productDb.Weight, 0, basket.ItemContainerList[i].ItemList[j].Quantity, productDb.Price,
                        orderBL.GetAdditionalComponentsPrice(additionalComponentDbList)));
                }

                orderBL.SetPromotionValues(itemsContainer, promotionId);
                foreach (var item in itemsContainer.ProductVMList)
                {
                    item.FinalValue = item.ProductValue + item.AdditionalComponentsValue;
                    baseOrderPrice += item.FinalValue;
                }

                basketVM.ItemsContainerList.Add(itemsContainer);
            }
            
            basketVM.OrderValue = orderBL.GetOrderValue(baseOrderPrice, User.IsInRole("Member"),
                _userManager.GetUserId(User), _context);

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
            var basket = Models.SessionExtensions.Get<List<Tuple<int, List<int>, int>>>(HttpContext.Session, "Basket");
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
                UserDb userDb = _context.UserDb.Single(x => x.AspNetUserId.Equals(user.Id));

                deliveryFormVM.ClientName = user.UserName;
                deliveryFormVM.Email = user.Email;
                deliveryFormVM.Phone = Int32.Parse(user.PhoneNumber);
                deliveryFormVM.City = userDb.City;
                deliveryFormVM.Street = userDb.Street;
                deliveryFormVM.HouseNumber = userDb.HouseNumber;
                deliveryFormVM.FlatNumber = userDb.FlatNumber;
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
                var basket = Models.SessionExtensions.Get<List<Tuple<int, List<int>, int>>>(HttpContext.Session, "Basket");
                if (basket == null || basket.Count() == 0)
                {
                    return View("EmptyBasket");
                }

                OrderBusinessLayer orderBL = new OrderBusinessLayer();
                List<OrderedProduct> orderedProductList = orderBL.GetOrderedProductList(basket, _context);
                decimal orderProductionCost = 0;    //production cost of ordered products

                for (int i = 0; i < basket.Count(); i++)
                {
                    orderProductionCost += (orderBL.GetProductProductionCost(basket[i].Item1, basket[i].Item2, _context)) * basket[i].Item3;
                }
                
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
                order.Value = orderBL.GetOrderValue(orderedProductList.Sum(x => x.FinalValue), User.IsInRole("Member"),
                    _userManager.GetUserId(User), _context, true);
                order.City = deliveryFormVM.City;
                order.Street = deliveryFormVM.Street;
                order.HouseNumber = deliveryFormVM.HouseNumber;
                order.FlatNumber = deliveryFormVM.FlatNumber;
                order.Profit = order.Value - orderProductionCost;

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
            CurrentOrdersViewModel currentOrdersVM = new CurrentOrdersViewModel();
            List<Order> currentOrderList = _context.Order.Where(x => x.Completed == false).ToList();
            var isAjax = Request.Headers["X-Requested-With"] == "XMLHttpRequest";

            foreach (var item in currentOrderList)
            {
                CurrentOrdersViewModel.CurrentOrder currentOrder = new CurrentOrdersViewModel.CurrentOrder(item);
                
                var orderedProductList = _context.OrderedProduct
                    .Where(x => x.OrderId == item.ID)
                    .Select(i => new { i.Name, i.AdditionalComponents, i.Size, i.FinalValue, i.Quantity })
                    .ToList();

                foreach (var orderedProduct in orderedProductList)
                {
                    ProductViewModel productVM = new ProductViewModel();

                    productVM.ProductName = orderedProduct.Name;
                    productVM.Size = orderedProduct.Size;
                    productVM.AdditionalComponents = orderedProduct.AdditionalComponents;
                    productVM.FinalValue = orderedProduct.FinalValue;
                    productVM.Quantity = orderedProduct.Quantity;

                    currentOrder.ProductList.Add(productVM);
                }

                if (currentOrdersVM.CurrentOrderList.Count() % 2 == 0)
                    currentOrder.BackgroundColor = "lightblue";
                else
                    currentOrder.BackgroundColor = "lightcyan";

                if (item.Sound)
                {
                    if(isAjax)
                    {
                        currentOrdersVM.Sound = true;
                        currentOrder.HtmlClass = "animation";
                    }

                    item.Sound = false;
                    _context.Update(item);
                    _context.SaveChanges();
                }

                currentOrdersVM.CurrentOrderList.Add(currentOrder);
            }

            if (isAjax)
            {
                return PartialView("_CurrentOrdersPartial", currentOrdersVM);
            }
            else
            {
                return View(currentOrdersVM);
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
                    .Select(x => new { x.Name, x.Components, x.AdditionalComponents, x.Size, x.Weight, x.FinalValue, x.Quantity })
                    .ToList();

                for (int j = 0; j < orderedProductList.Count(); j++)
                {
                    ProductViewModel productVM = new ProductViewModel(
                        orderedProductList[j].Name,
                        orderedProductList[j].Components,
                        orderedProductList[j].AdditionalComponents,
                        orderedProductList[j].Size,
                        orderedProductList[j].Weight,
                        orderedProductList[j].FinalValue,
                        orderedProductList[j].Quantity
                    );

                    myOrderVM.ProductList.Add(productVM);
                }

                myOrderVM.Value = myOrderDbList[i].Value;
                myOrderVM.OrderDate = myOrderDbList[i].Date;

                myOrderVMList.Add(myOrderVM);
            }

            return View(myOrderVMList);
        }


        [HttpPost]
        public IActionResult EditProductInBasket(int productIndex, string quantityBtn)
        {
            if(quantityBtn.Equals("+"))
                Models.SessionExtensions.EditProductQuantity(HttpContext.Session, "Basket", productIndex, true, false);
            else if(quantityBtn.Equals("-"))
                Models.SessionExtensions.EditProductQuantity(HttpContext.Session, "Basket", productIndex, false, true);

            int quantity = Models.SessionExtensions.Get<List<Tuple<int, List<int>, int>>>(HttpContext.Session, "Basket")[productIndex].Item3;

            return RedirectToAction("Basket");
        }


        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);
    }
}