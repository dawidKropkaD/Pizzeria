using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzeria.Models
{
    public class Address
    {
        [Display(Name = "Miasto")]
        public virtual string City { get; set; }

        [Display(Name = "Ulica")]
        public virtual string Street { get; set; }

        [Display(Name = "Nr domu")]
        public virtual string HouseNumber { get; set; }

        [Display(Name = "Nr mieszkania")]
        public virtual string FlatNumber { get; set; }
    }
}
