using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceAbstraction;
using Shared.Dtos.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController(ICustomerService service) : ControllerBase
    {
        [HttpPost]
        [Authorize(Roles = "Admin,Customer")]
        public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerDto customerDto)
        {
            var UserId = User.FindFirstValue(claimType: ClaimTypes.NameIdentifier);
            var res = await service.CreateCustomer(customerDto, UserId!);
            return Ok(res);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Customer")]
        public async Task<IActionResult> GetAllOrders(int ID)
        {
            var res = await service.GetOrderOfCustomer(ID);
            return Ok(res);
        }
    }
}
