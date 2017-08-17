using Pizzeria.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzeria.Models
{
    public class Basket
    {
        public List<ItemContainer> ItemContainerList { get; set; }


        public Basket()
        {
            ItemContainerList = new List<ItemContainer>();
        }



        //ITEMCONTAINER CLASS
        public class ItemContainer
        {
            public List<Item> ItemList { get; set; }
            public int? PromotionId { get; set; }
            

            public ItemContainer()
            {
                ItemList = new List<Item>();
            }
            
            public override bool Equals(object obj)
            {
                if (obj is ItemContainer == false)
                    return base.Equals(obj);

                ItemContainer itemContainer = (ItemContainer)obj;

                if (Nullable.Equals(PromotionId, itemContainer.PromotionId) == false)
                    return false;
                if (ItemList.Count() != itemContainer.ItemList.Count())
                    return false;

                for (int i = 0; i < ItemList.Count(); i++)
                {
                    if (ItemList[i].Equals(itemContainer.ItemList[i]) == false)
                    {
                        return false;
                    }
                }
                return true;
            }

            public override int GetHashCode()
            {
                return PromotionId.GetHashCode();
            }
        }



        //ITEM CLASS
        public class Item
        {
            public int ProductId { get; set; }
            public List<int> AdditionalComponentIdList { get; set; }
            public int Quantity { get; set; }


            public Item()
            {
                AdditionalComponentIdList = new List<int>();
            }
            
            public override bool Equals(object obj)
            {
                if (obj is Item == false)
                    return base.Equals(obj);

                Item item = (Item)obj;

                if (ProductId != item.ProductId)
                    return false;

                if (AdditionalComponentIdList == null && item.AdditionalComponentIdList == null)
                    return true;

                if (AdditionalComponentIdList == null || item.AdditionalComponentIdList == null)
                    return false;

                if (AdditionalComponentIdList.Count() != item.AdditionalComponentIdList.Count())
                    return false;

                AdditionalComponentIdList.Sort();
                item.AdditionalComponentIdList.Sort();

                for (int i = 0; i < AdditionalComponentIdList.Count(); i++)
                {
                    if (AdditionalComponentIdList[i] != item.AdditionalComponentIdList[i])
                        return false;
                }

                return true;
            }

            public override int GetHashCode()
            {
                return ProductId.GetHashCode();
            }
        }
    }
}
