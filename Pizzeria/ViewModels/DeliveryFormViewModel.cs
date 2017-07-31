using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzeria.ViewModels
{
    public class DeliveryFormViewModel
    {
        [Required]
        public string City { get; set; }

        [Required]
        public string Street { get; set; }

        [Required]
        public string HouseNumber { get; set; }

        public string FlatNumber { get; set; }

        [Required]
        public string ClientName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^([0-9]{9})$")]
        public int Phone { get; set; }

        /// <summary>
        /// Member is the user role
        /// </summary>
        public string DisplayForMember { get; set; }
    }
}


