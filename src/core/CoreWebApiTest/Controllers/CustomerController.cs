using CoreWebApiTest.Contracts;
using CoreWebApiTest.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApiTest.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [Produces("application/json")]
    public class CustomerController : Controller
    {
        private ICustomerService _customerService { get; set; }

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet("list")]
        [Produces(typeof(CustomerModel[]))]
        public IActionResult List()
        {
            return Ok(_customerService.List());
        }

        [HttpPost("")]
        public IActionResult Create([FromBody] CustomerModel model)
        {
            _customerService.Create(model);
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _customerService.Delete(id);
            return Ok();
        }
    }
}
