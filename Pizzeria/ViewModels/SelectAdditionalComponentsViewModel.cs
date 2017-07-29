using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzeria.ViewModels
{
    public class SelectAdditionalComponentsViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductComponents { get; set; }
        public decimal PriceOfOrder { get; set; }

        /// <summary>
        /// Item1: id, item2: name, item3: price
        /// </summary>
        public List<Tuple<int, string, decimal>> AdditionalComponentDetails{ get; set; }

        public SelectAdditionalComponentsViewModel()
        {
            AdditionalComponentDetails = new List<Tuple<int, string, decimal>>();
        }
    }
}
