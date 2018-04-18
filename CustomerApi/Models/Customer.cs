using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApi.Models
{
    public class Customer
    {
        public string Name { get; set; }
        [Key]
        public int RegistrationNumber { get; set; }
        public string Email { get; set; }
        public double Phone { get; set; }
        public string BillingAddress { get; set; }
        public string ShippingAddress { get; set; }

    }
}
