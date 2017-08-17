using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzeria.Models.Tables
{
    public class Promotion
    {
        public int ID { get; set; }

        [EnumDataType(typeof(PromotionType))]
        public PromotionType Type { get; set; }

        public string Description { get; set; }

        public string ShortDescription { get; set; }
        
        public string ProductCategory { get; set; }



        public enum PromotionType
        {
            NoPromotion = 0,
            Discount20 = 1,
            DrinkForFree = 2,
            SecondProductFor199 = 3
        }
    }
}
