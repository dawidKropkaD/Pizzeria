using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzeria.Models.AccountViewModels
{
    public class RegisterViewModel : Address
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Potwierdź hasło")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "Imię")]
        public string UserName { get; set; }

        [Required]
        public override string City { get; set; }

        [Required]
        public override string Street { get; set; }

        [Required]
        public override string HouseNumber { get; set; }

        [Display(Name = "Nr telefonu")]
        [Required]
        [RegularExpression(@"^([0-9]{9})$")]
        public string PhoneNumber { get; set; }
    }
}
