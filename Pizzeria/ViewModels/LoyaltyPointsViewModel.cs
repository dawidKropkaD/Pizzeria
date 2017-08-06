using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzeria.ViewModels
{
    public class LoyaltyPointsViewModel
    {
        [Display(Name = "Liczba Twoich punktów lojalnościowych wynosi")]
        public int LoyaltyPoints { get; set; }

        [Display(Name = "Dostępne środki pieniężne")]
        [DisplayFormat(DataFormatString = "{0:n2} zł")]
        public decimal MoneyPrize { get; set; }
    }
}
