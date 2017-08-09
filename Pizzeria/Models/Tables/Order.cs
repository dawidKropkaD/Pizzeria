using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzeria.Models.Tables
{
    public class Order
    {
        public int ID { get; set; }

        [Display(Name = "Imię klienta")]
        public string ClientName { get; set; }

        public string UserEmail { get; set; }

        [Display(Name = "Telefon")]
        public int Phone { get; set; }

        public DateTime Date { get; set; }

        [Display(Name = "Wartość zamówienia")]
        [DisplayFormat(DataFormatString = "{0:n2} zł")]
        public decimal Value { get; set; }

        public string City { get; set; }

        public string Street { get; set; }

        public string HouseNumber { get; set; }

        public string FlatNumber { get; set; }

        [Display(Name = "Zrealizowane")]
        public bool Completed { get; set; }

        public virtual bool Sound { get; set; } = true;
    }
}
