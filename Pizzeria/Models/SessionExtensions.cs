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
        public static void Add(this ISession session, string key, Tuple<int, List<int>> value)
        {
            var old = Get<List<Tuple<int, List<int>>>>(session, key);

            if (old == null)
            {
                List<Tuple<int, List<int>>> valueList = new List<Tuple<int, List<int>>>();
                valueList.Add(value);
                session.SetString(key, JsonConvert.SerializeObject(valueList));
            }
            else
            {
                old.Add(value);
                session.SetString(key, JsonConvert.SerializeObject(old));
            }
        }
        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }

        public static void DeleteProduct(this ISession session, string key, int productIndex)
        {
            var sessionValue = Get<List<Tuple<int, List<int>>>>(session, key);

            if (sessionValue == null || sessionValue.Count() == 0)
            {
                return;
            }

            sessionValue.RemoveAt(productIndex);
            session.SetString(key, JsonConvert.SerializeObject(sessionValue));
        }
    }
}
