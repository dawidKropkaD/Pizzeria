using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Pizzeria.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzeria.Models
{
    public static class SessionExtensions
    {
        public static void AddContainer(this ISession session, int productId, List<int> additionalComponentIdList, int? promotionId)
        {
            string key = "Basket";
            Basket.ItemContainer itemContainer = new Basket.ItemContainer();
            Basket.Item item = new Basket.Item();

            item.ProductId = productId;
            item.AdditionalComponentIdList = additionalComponentIdList;
            item.Quantity = 1;

            itemContainer.ItemList.Add(item);
            itemContainer.PromotionId = promotionId;
                        
            var basket = Get2(session, key);
            if (basket == null)
            {
                basket = new Basket();
                
                basket.ItemContainerList.Add(itemContainer);

                session.SetString(key, JsonConvert.SerializeObject(basket));
            }
            else
            {
                OrderBusinessLayer orderBL = new OrderBusinessLayer();
                orderBL.MergeDuplicateItemsContainers(basket.ItemContainerList, itemContainer);

                session.SetString(key, JsonConvert.SerializeObject(basket));
            }
        }


        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }

        public static Basket Get2(this ISession session, string key)
        {
            var value = session.GetString(key);

            return value == null ? null : JsonConvert.DeserializeObject<Basket>(value);
        }

        public static void DeleteProduct(this ISession session, string key, int productIndex)
        {
            var sessionValue = Get<List<Tuple<int, List<int>, int>>>(session, key);

            if (sessionValue == null || sessionValue.Count() == 0)
            {
                return;
            }

            sessionValue.RemoveAt(productIndex);
            session.SetString(key, JsonConvert.SerializeObject(sessionValue));
        }

        public static void EditProductQuantity(this ISession session, string key, int productIndex, bool increment, bool decrement)
        {
            List<Tuple<int, List<int>, int>> sessionValue = Get<List<Tuple<int, List<int>, int>>>(session, key);

            if (sessionValue == null || sessionValue.Count() == 0 
                || (sessionValue[productIndex].Item3 == 1 && decrement) || (increment && decrement))
            {
                return;
            }

            if(increment)
                sessionValue[productIndex] = new Tuple<int, List<int>, int>(
                    sessionValue[productIndex].Item1,
                    sessionValue[productIndex].Item2,
                    sessionValue[productIndex].Item3 + 1);
            if(decrement)
                sessionValue[productIndex] = new Tuple<int, List<int>, int>(
                    sessionValue[productIndex].Item1,
                    sessionValue[productIndex].Item2,
                    sessionValue[productIndex].Item3 - 1);

            session.SetString(key, JsonConvert.SerializeObject(sessionValue));
        }
    }
}
