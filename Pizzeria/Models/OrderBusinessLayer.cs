using Microsoft.AspNetCore.Http;
using Pizzeria.Data;
using Pizzeria.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;

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

        public List<OrderedProduct> GetOrderedProductList(List<Tuple<int, List<int>>> basket, ApplicationDbContext _context)
        {
            List<OrderedProduct> orderedProductList = new List<OrderedProduct>();

            for (int i = 0; i < basket.Count(); i++)
            {
                OrderedProduct orderedProduct = new OrderedProduct();
                List<AdditionalComponent> additionalComponentList = _context.AdditionaComponent.Where(x => basket[i].Item2.Contains(x.ID)).ToList();
                decimal baseProductPrice = _context.ProductDb.SingleOrDefault(x => x.ID == basket[i].Item1).Price;
                decimal additionalComponentsPrice = additionalComponentList.Sum(x => x.Price);

                orderedProduct.ProductId = basket[i].Item1;
                orderedProduct.AdditionalComponents = GetAdditionalComponentsName(additionalComponentList);
                orderedProduct.Value = baseProductPrice + additionalComponentsPrice;

                orderedProductList.Add(orderedProduct);
            }

            return orderedProductList;
        }
    }
}
