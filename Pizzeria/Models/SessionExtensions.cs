using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzeria.Models
{
    public static class SessionExtensions
    {
        /// <param name="value">Item1: product id, item2: list of additional components ids, item3: quantity</param>
        public static void AddProduct(this ISession session, string key, Tuple<int, List<int>, int> product)
        {
            var oldBasket = Get<List<Tuple<int, List<int>, int>>>(session, key);

            if (oldBasket == null)
            {
                List<Tuple<int, List<int>, int>> basket = new List<Tuple<int, List<int>, int>>();
                basket.Add(product);
                session.SetString(key, JsonConvert.SerializeObject(basket));
            }
            else
            {
                OrderBusinessLayer orderBL = new OrderBusinessLayer();
                var currentBasket = orderBL.MergeDuplicateProducts(oldBasket, product);

                session.SetString(key, JsonConvert.SerializeObject(currentBasket));
            }
        }
        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
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
