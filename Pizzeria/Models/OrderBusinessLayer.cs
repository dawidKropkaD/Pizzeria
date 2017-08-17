using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Pizzeria.Data;
using Pizzeria.Models.Tables;
using Pizzeria.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;
using Microsoft.Extensions.Configuration;

namespace Pizzeria.Models
{
    public class OrderBusinessLayer
    {
        public string GetAdditionalComponentsName(List<AdditionalComponent> additionalComponentList)
        {
            var nameList = additionalComponentList.Select(x => x.Name);
            if (nameList == null || nameList.Count() == 0)
            {
                return null;
            }

            return string.Join(", ", nameList.ToArray());
        }

        public decimal GetAdditionalComponentsPrice(List<AdditionalComponent> additionalComponentList)
        {
            decimal price = 0;

            for (int i = 0; i < additionalComponentList.Count(); i++)
            {
                price += additionalComponentList[i].Price;
            }

            return price;
        }

        public List<OrderedProduct> GetOrderedProductList(List<Tuple<int, List<int>, int>> basket, ApplicationDbContext _context)
        {
            List<OrderedProduct> orderedProductList = new List<OrderedProduct>();

            for (int i = 0; i < basket.Count(); i++)
            {
                OrderedProduct orderedProduct = new OrderedProduct();
                var productDb = _context.ProductDb
                    .Where(x => x.ID == basket[i].Item1)
                    .Select(x => new { x.ProductName, x.Components, x.Size, x.Weight, x.Price })
                    .SingleOrDefault();
                List<AdditionalComponent> additionalComponentList = _context.AdditionaComponent.Where(x => basket[i].Item2.Contains(x.ID)).ToList();
                decimal additionalComponentsPrice = additionalComponentList.Sum(x => x.Price);

                orderedProduct.Quantity = basket[i].Item3;
                orderedProduct.Name = productDb.ProductName;
                orderedProduct.Components = productDb.Components;
                orderedProduct.AdditionalComponents = GetAdditionalComponentsName(additionalComponentList);
                orderedProduct.Size = productDb.Size;
                orderedProduct.Weight = productDb.Weight;
                orderedProduct.FinalValue = (productDb.Price + additionalComponentsPrice) * orderedProduct.Quantity;
                
                orderedProductList.Add(orderedProduct);
            }

            return orderedProductList;
        }

        /// <summary>
        /// Get order value include money prize
        /// </summary>
        public decimal GetOrderValue(decimal baseOrderPrice, bool userIsMember, string userId, ApplicationDbContext _context, bool updateMoneyPrize = false)
        {
            baseOrderPrice = (baseOrderPrice < 20) ? 20 : baseOrderPrice;

            if (!userIsMember)
            {
                return baseOrderPrice;
            }

            UserDb userDb = _context.UserDb.Where(x => x.AspNetUserId.Equals(userId)).Single();
            decimal finalPrice;

            if (baseOrderPrice >= userDb.MoneyPrize)
            {
                finalPrice = baseOrderPrice - userDb.MoneyPrize;
                userDb.MoneyPrize = 0;
            }
            else
            {
                finalPrice = 0;
                userDb.MoneyPrize -= baseOrderPrice;
            }

            if (updateMoneyPrize)
            {
                _context.Update(userDb);
                _context.SaveChanges();
            }
            
            return finalPrice;
        }

        public void AddLoyaltyPoints(int loyaltyPointsNumber, bool userIsMember, string userId, ApplicationDbContext _context)
        {
            
            if (!userIsMember)
            {
                return;
            }
            int threshold = 100;
            int loyaltyPointValue = 1;  //w złotówkach

            UserDb userDb = _context.UserDb.Single(x => x.AspNetUserId.Equals(userId));

            int currentLoayltyPoints = userDb.LoyaltyPoints + loyaltyPointsNumber;

            if (currentLoayltyPoints >= threshold)
            {
                int quotient = (int)Math.Floor((1.0 * currentLoayltyPoints / threshold));
                userDb.LoyaltyPoints = currentLoayltyPoints - quotient * threshold;
                userDb.MoneyPrize += quotient * threshold * loyaltyPointValue;
            }
            else
            {
                userDb.LoyaltyPoints = currentLoayltyPoints;
            }

            _context.Update(userDb);
            _context.SaveChanges();
        }

        public List<Tuple<int, List<int>, int>> MergeDuplicateProducts(List<Tuple<int, List<int>, int>> productList, Tuple<int, List<int>, int> newProduct)
        {
            for (int i = 0; i < productList.Count(); i++)
            {
                if (ProductsAreEquals(productList[i].Item1, productList[i].Item2, newProduct.Item1, newProduct.Item2))
                {
                    productList[i] = new Tuple<int, List<int>, int>(productList[i].Item1, productList[i].Item2, productList[i].Item3 + newProduct.Item3);

                    return productList;
                }
            }

            productList.Add(new Tuple<int, List<int>, int>(newProduct.Item1, newProduct.Item2, newProduct.Item3));

            return productList;
        }

        [System.Obsolete("ProductsAreEquals is duplicated with Equals in Basket.ItemContainer class, I recommend use Equals.")]
        public bool ProductsAreEquals(int productId1, List<int> additionalComponentsIds1, int productId2, List<int> additionalComponentsIds2)
        {
            if (productId1 != productId2)
                return false;

            if (additionalComponentsIds1 == null && additionalComponentsIds2 == null)
                return true;

            if (additionalComponentsIds1 == null || additionalComponentsIds2 == null)
                return false;

            if (additionalComponentsIds1.Count() != additionalComponentsIds2.Count())
                return false;

            additionalComponentsIds1.Sort();
            additionalComponentsIds2.Sort();

            for (int i = 0; i < additionalComponentsIds1.Count(); i++)
            {
                if (additionalComponentsIds1[i] != additionalComponentsIds2[i])
                    return false;
            }

            return true;
        }

        public decimal GetProductProductionCost(int productId, List<int> additionalComponentIdList, ApplicationDbContext context)
        {
            decimal productionCost = 0;
            var productDb = context.ProductDb
                .Where(x => x.ID == productId)
                .Select(i => new { i.Price, i.Profit })
                .SingleOrDefault();

            productionCost = productDb.Price - productDb.Profit;

            if (additionalComponentIdList == null || additionalComponentIdList.Count() == 0)
            {
                return productionCost;
            }

            for (int i = 0; i < additionalComponentIdList.Count(); i++)
            {
                var additionalComponentDb = context.AdditionaComponent
                    .Where(x => x.ID == additionalComponentIdList[i])
                    .Select(y => new { y.Price, y.Profit })
                    .SingleOrDefault();

                productionCost += additionalComponentDb.Price - additionalComponentDb.Profit;
            }

            return productionCost;
        }

        public ProductDb TryGetOnlineProduct(ApplicationDbContext context, string name, string category, decimal price)
        {
            if (name == null || category == null)
            {
                return null;
            }

            var productDb = context.ProductDb
                .SingleOrDefault(x => x.ProductName.Equals(name) && x.Category.Equals(category) && x.Price == price && x.IsOnline == true);

            if (productDb == null)
            {
                return null;
            }

            return productDb;
        }

        public ProductDb TryGetOnlineProduct(ApplicationDbContext context, int? id)
        {
            if (id == null)
            {
                return null;
            }

            var productDb = context.ProductDb.SingleOrDefault(x => x.ID == id && x.IsOnline == true);

            if (productDb == null)
            {
                return null;
            }

            return productDb;
        }

        public List<Basket.ItemContainer> MergeDuplicateItemsContainers(List<Basket.ItemContainer> containerList, Basket.ItemContainer newContainer)
        {
            for (int i = 0; i < containerList.Count(); i++)
            {
                if (containerList[i].Equals(newContainer))
                {
                    for (int j = 0; j < containerList[i].ItemList.Count(); j++)
                    {
                        containerList[i].ItemList[i].Quantity += newContainer.ItemList[j].Quantity;
                    }

                    return containerList;
                }
            }

            containerList.Add(newContainer);

            return containerList;
        }

        public bool SelectedComponentsValid(ApplicationDbContext context, List<int> selectedComponentList, string productCategory)
        {
            if (selectedComponentList == null || selectedComponentList.Count() == 0)
            {
                return true;
            }

            if (productCategory == null)
            {
                return false;
            }

            var availableAdditionalComponentsIds = context.AdditionaComponent.Where(x => x.Category.Equals(productCategory)).Select(x => x.ID).ToList();

            for (int i = 0; i < selectedComponentList.Count(); i++)
            {
                if (availableAdditionalComponentsIds.Contains(selectedComponentList[i]) == false)
                {
                    return false;
                }
            }

            return true;
        }
        
        /// <param name="productCategory">Product concerned with promotion</param>
        public bool SelectedPromotionIdValid(ApplicationDbContext context, int? promotionId, string productCategory)
        {
            if (promotionId == null)
                return true;

            var availablePromotionsIds = context.Promotion
                    .Where(x => x.ProductCategory.Equals(productCategory))
                    .Select(x => x.ID).ToList();

            if (availablePromotionsIds.Contains((int)promotionId) == false)
            {
                return false;
            }

            return true;
        }

        public Promotion.PromotionType GetPromotionType(ApplicationDbContext context, int? promotionId)
        {
            if (promotionId == null)
                return Promotion.PromotionType.NoPromotion;

            return context.Promotion.Single(x => x.ID == (int)promotionId).Type;
        }

        public void SetPromotionValues(BasketViewModel.ItemsContainer itemsContainer, int? promotionId)
        {
            if (promotionId == null)
                return;
        }
    }
}
