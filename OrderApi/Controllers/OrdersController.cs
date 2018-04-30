using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OrderApi.Data;
using OrderApi.Models;
using RestSharp;

namespace OrderApi.Controllers
{
    [Route("api/orders")]
    public class OrdersController : Controller
    {
        private readonly IRepository<Order> repository;

        public OrdersController(IRepository<Order> repos)
        {
            repository = repos;
        }

        // GET: api/orders/
        [HttpGet]
        public IEnumerable<Order> Get()
        {
            return repository.GetAll();
        }

        // GET api/orders/5
        [HttpGet("{id}", Name = "GetOrder")]
        public IActionResult Get(int id)
        {
            var item = repository.Get(id);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }

        // POST api/orders
        [HttpPost]
        public IActionResult Post([FromBody]Order order)
        {
            if (order == null)
            {
                return BadRequest();
            }

            // Call ProductApi to get the product ordered
            RestClient c = new RestClient();
            // You may need to change the port number in the BaseUrl below
            // before you can run the request.
            //c.BaseUrl = new Uri("http://localhost:8000/api/customers/");
            c.BaseUrl = new Uri("http://customerapi/api/customers/");
            var customerRequest = new RestRequest(order.CustomerRN.ToString(), Method.GET);
            var customerResponse = c.Execute<Customer>(customerRequest);
            var customerInOrder = customerResponse.Data;
            if (!customerInOrder.HasOutstanding)
            {
                // You may need to change the port number in the BaseUrl below
                // before you can run the request.
                c = new RestClient();
                //c.BaseUrl = new Uri("http://localhost:8002/api/products/");
                c.BaseUrl = new Uri("http://productapi/api/products/");
                var productRequest = new RestRequest(order.ProductId.ToString(), Method.GET);
                var productResponse = c.Execute<Product>(productRequest);
                var orderedProduct = productResponse.Data;

                if (order.Quantity <= orderedProduct.ItemsInStock)
                {
                    // reduce the number of items in stock for the ordered product,
                    // and create a new order.
                    orderedProduct.ItemsInStock -= order.Quantity;
                    var updateRequest = new RestRequest(orderedProduct.Id.ToString(), Method.PUT);
                    updateRequest.AddJsonBody(orderedProduct);
                    var updateResponse = c.Execute(updateRequest);

                    if (updateResponse.IsSuccessful)
                    {
                        var newOrder = repository.Add(order);
                        return CreatedAtRoute("GetOrder", new {id = newOrder.Id}, newOrder);
                    }
                }
            }

            // If the order could not be created, "return no content".
            return NoContent();
        }

        // PUT api/products/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]Order order)
        {
            if (order == null || order.Id != id)
            {
                return BadRequest();
            }

            var modifiedOrder = repository.Get(id);

            if (modifiedOrder == null)
            {
                return NotFound();
            }
            
            modifiedOrder.IsShipped = order.IsShipped;
            modifiedOrder.ProductId = order.ProductId;
            modifiedOrder.Quantity = order.Quantity;

            repository.Edit(modifiedOrder);
            return new NoContentResult();
        }

        // DELETE api/products/5
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
