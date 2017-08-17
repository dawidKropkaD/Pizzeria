using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzeria.ViewModels
{
    public class SelectPromotionViewModel
    {
        public List<SelectPromotion> PossiblePromotionList { get; set; }
        public int ProductId { get; set; }


        public SelectPromotionViewModel()
        {
            PossiblePromotionList = new List<SelectPromotion>();
        }



        public class SelectPromotion
        {
            public int PromotionId { get; set; }
            public string PromotionDescription { get; set; }
            public string PromotionShortDescription { get; set; }


            public SelectPromotion()
            {
                
            }

            public SelectPromotion(int id, string description, string shortDescription)
            {
                PromotionId = id;
                PromotionDescription = description;
                PromotionShortDescription = shortDescription;
            }
        }
    }
}
