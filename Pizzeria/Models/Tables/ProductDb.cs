using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzeria.Models
{
    public class ProductDb
    {
        public int ID { get; set; }

        [Required]
        [Display(Name = "Nazwa")]
        public string ProductName { get; set; }


        [Required]
        [Display(Name = "Kategoria")]
        public string Category { get; set; }


        [Display(Name = "Podkategoria")]
        public string SubCategory { get; set; }


        [Display(Name = "Składniki")]
        public string Components { get; set; }


        [Display(Name = "Cena")]
        public decimal Price { get; set; }

        /// <summary>
        /// In centimeters
        /// </summary>
        [Display(Name = "Rozmiar (w centymetrach)")]
        public double? Size { get; set; }


        /// <summary>
        /// In gram
        /// </summary>
        [Display(Name = "Waga (w gramach)")]
        public double? Weight { get; set; }


        [Display(Name = "Dostępny w lokalu")]
        public bool IsInLocal { get; set; }


        [Display(Name = "Dostępny online")]
        public bool IsOnline { get; set; }


        /// <summary>
        /// In zloty
        /// </summary>
        [Display(Name = "Marża (w złotówkach)")]
        [DisplayFormat(DataFormatString = "{0:n2} zł")]
        public decimal Profit { get; set; }
    }
}
