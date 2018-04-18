using System.Collections.Generic;
using System.Linq;
using CustomerApi.Models;

namespace CustomerApi.Data
{
    public static class DbInitializer
    {
        // This method will create and seed the database.
        public static void Initialize(CustomerApiContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            // Look for any Customers
            if (context.Customers.Any())
            {
                return;   // DB has been seeded
            }

            List<Customer> customers = new List<Customer>
            {
                new Customer { Name= "Jhon Doe",RegistrationNumber= 1, Phone= 12313123, Email ="dsafasdas", BillingAddress="dasdadasd", ShippingAddress="ffegefa"},
                new Customer { Name= "Jhon Doe2",RegistrationNumber= 2, Phone= 12313123, Email ="dsafasdas", BillingAddress="dasdadasd", ShippingAddress="ffegefa"},
            };

            context.Customers.AddRange(customers);
            context.SaveChanges();
        }
    }
}
