using System.Collections.Generic;
using System.Linq;
using OrderApi.Models;
using System;

namespace OrderApi.Data
{
    public static class DbInitializer
    {
        // This method will create and seed the database.
        public static void Initialize(OrderApiContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            // Look for any Products
            if (context.Orders.Any())
            {
                return;   // DB has been seeded
            }

            List<Order> orders = new List<Order>
            {
                new Order { Date = DateTime.Today, ProductId = 2, Quantity = 2, CustomerRN=1, IsShipped=false }
            };

            context.Orders.AddRange(orders);
            context.SaveChanges();
        }
    }
}
