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
        public string ProductName { get; set; }

        [Required]
        public string Category { get; set; }

        public string SubCategory { get; set; }

        public string Components { get; set; }

        public decimal Price { get; set; }

        /// <summary>
        /// In centimeters
        /// </summary>
        public double? Size { get; set; }

        /// <summary>
        /// In gram
        /// </summary>
        public double? Weight { get; set; }

        public bool IsInLocal { get; set; }

        public bool IsOnline { get; set; }
    }
}
