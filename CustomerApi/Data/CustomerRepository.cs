﻿using CustomerApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApi.Data
{
    public class CustomerRepository : IRepository<Customer>
    {
        private readonly CustomerApiContext db;

        public CustomerRepository(CustomerApiContext context)
        {
            db = context;
        }

        public Customer Add(Customer entity)
        {
            var newProduct = db.Customers.Add(entity).Entity;
            db.SaveChanges();
            return newProduct;
        }

        public void Edit(Customer entity)
        {
            db.Entry(entity).State = EntityState.Modified;
            db.SaveChanges();
        }
        
        public Customer Get(int id)
        {
            return db.Customers.FirstOrDefault(c => c.RegistrationNumber == id);
        }

        public IEnumerable<Customer> GetAll()
        {
            return db.Customers.ToList();
        }

        public void Remove(int id)
        {
            var product = db.Customers.FirstOrDefault(p => p.RegistrationNumber == id);
            db.Customers.Remove(product);
            db.SaveChanges();
        }
    }
}
