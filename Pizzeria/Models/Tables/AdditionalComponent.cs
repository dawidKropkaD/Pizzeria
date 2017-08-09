using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzeria.Models
{
    public class AdditionalComponent
    {
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Category { get; set; }

        public decimal Price { get; set; }

        /// <summary>
        /// In zloty
        /// </summary>
        public decimal Profit { get; set; }
    }
}
