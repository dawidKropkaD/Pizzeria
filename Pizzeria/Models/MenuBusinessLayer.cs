using Pizzeria.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzeria.Models
{
    public class MenuBusinessLayer
    {
        private ApplicationDbContext _context;

        public MenuBusinessLayer(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Product> ConvertProductDbToProduct(List<ProductDb> productsDb)
        {
            List<Product> products = new List<Product>();

            foreach (var item in productsDb)
            {
                if (!item.Category.Equals("Pizza") || products.Where(x => x.ProductName.Equals(item.ProductName)).Count() == 0)
                {
                    Product p = new Product();

                    p.ProductName = item.ProductName;
                    p.Category = item.Category;
                    p.SubCategory = item.SubCategory;
                    p.Components = item.Components;
                    p.Price.Add(item.Price);
                    p.Weight = item.Weight;
                    p.IsInLocal = item.IsInLocal;
                    p.IsOnline = item.IsOnline;

                    if (item.Size != null)
                        p.Size.Add((double)item.Size);
                    
                    products.Add(p);
                }
                else
                {
                    Product pizza = products.Where(x => x.ProductName.Equals(item.ProductName)).First();
                    pizza.Price.Add(item.Price);
                    if (item.Size != null)
                        pizza.Size.Add((double)item.Size);
                }
            }

            //Sort price and size
            foreach (var item in products)
            {
                item.Price.Sort();
                item.Size.Sort();
            }

            return products;
        }
    }
}
