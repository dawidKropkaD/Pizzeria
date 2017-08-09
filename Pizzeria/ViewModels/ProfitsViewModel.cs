using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzeria.ViewModels
{
    public class ProfitsViewModel
    {
        public List<Profit> ProfitList { get; set; }
        
        [DisplayFormat(DataFormatString = "{0:n2} zł")]
        public decimal PaidForOrderSum { get; set; }
        

        /// <summary>
        /// In zloty
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:n2} zł")]
        public decimal ProfitSum { get; set; }


        public ProfitsViewModel()
        {
            ProfitList = new List<Profit>();
        }


        public class Profit
        {
            [Display(Name = "Data zamówienia")]
            [DisplayFormat(DataFormatString = "{0:d/MM/yyyy H:mm}")]
            public DateTime OrderDate { get; set; }


            [Display(Name = "Zapłacono za zamówienie")]
            [DisplayFormat(DataFormatString = "{0:n2} zł")]
            public decimal PaidForOrder { get; set; }


            /// <summary>
            /// In zloty
            /// </summary>
            [Display(Name = "Marża")]
            [DisplayFormat(DataFormatString = "{0:n2} zł")]
            public decimal OrderProfit { get; set; }



            public Profit()
            {
            }

            public Profit(DateTime orderDate, decimal paidForOrder, decimal orderProfit)
            {
                OrderDate = orderDate;
                PaidForOrder = paidForOrder;
                OrderProfit = orderProfit;
            }
        }
    }
}
