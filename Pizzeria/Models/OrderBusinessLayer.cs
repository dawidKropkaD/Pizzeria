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
                var productDb = _context.ProductDb
                    .Where(x => x.ID == basket[i].Item1)
                    .Select(x => new { x.ProductName, x.Components, x.Size, x.Weight, x.Price })
                    .SingleOrDefault();
                List<AdditionalComponent> additionalComponentList = _context.AdditionaComponent.Where(x => basket[i].Item2.Contains(x.ID)).ToList();
                decimal additionalComponentsPrice = additionalComponentList.Sum(x => x.Price);
                
                orderedProduct.Name = productDb.ProductName;
                orderedProduct.Components = productDb.Components;
                orderedProduct.AdditionalComponents = GetAdditionalComponentsName(additionalComponentList);
                orderedProduct.Size = productDb.Size;
                orderedProduct.Weight = productDb.Weight;
                orderedProduct.Value = productDb.Price + additionalComponentsPrice;

                orderedProductList.Add(orderedProduct);
            }

            return orderedProductList;
        }
    }
}
