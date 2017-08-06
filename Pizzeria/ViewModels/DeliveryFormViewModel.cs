using Microsoft.AspNetCore.Mvc.Rendering;
using Pizzeria.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzeria.ViewModels
{
    public class DeliveryFormViewModel : Address
    {
        [Required]
        public override string City { get; set; }

        [Required]
        public override string Street { get; set; }

        [Required]
        public override string HouseNumber { get; set; }

        public override string FlatNumber { get; set; }

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


