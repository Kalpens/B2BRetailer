using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomerApi.Data;
using CustomerApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace CustomerApi.Controllers
{

    [Route("api/customer")]
    public class CustomersController : Controller
    {
        private readonly IRepository<Customer> repository;

        public CustomersController(IRepository<Customer> repos)
        {
            repository = repos;
        }

        // GET api/customers
        [HttpGet]
        public IEnumerable<Customer> Get()
        {
            return repository.GetAll();
        }

        // GET api/customers/5
        [HttpGet("{id}", Name="GetCustomer")]
        public IActionResult Get(int id)
        {
            var item = repository.Get(id);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }

        // POST api/customers
        [HttpPost]
        public IActionResult Post([FromBody]Customer c)
        {
            if ( c == null)
            {
                return BadRequest();
            }

            var newCustomer = repository.Add(c);

            return CreatedAtRoute("GetCustomer", new { id = newCustomer.RegistrationNumber }, newCustomer);
        }

        // PUT api/customers/5
        [HttpPut("{id}")]
        public IActionResult Put (int id, [FromBody]Customer c)
        {
            if (c == null || c.RegistrationNumber != id)
            {
                return BadRequest();
            }

            var modifiedCustomer = repository.Get(id);

            if (modifiedCustomer == null)
            {
                return NotFound();
            }

            modifiedCustomer.Phone = c.Phone;
            modifiedCustomer.Email = c.Email;
            modifiedCustomer.BillingAddress = c.BillingAddress;
            modifiedCustomer.ShippingAddress = c.ShippingAddress;

            repository.Edit(modifiedCustomer);
            return new NoContentResult();
        }
        // DELETE api/customers/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (repository.Get(id) == null)
            {
                return NotFound();
            }

            repository.Remove(id);
            return new NoContentResult();
        }
    }
}
