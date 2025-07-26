using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using ServiceAbstraction;
using Shared.Dtos.ProductDto;
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
    public class ProductController(IProductService service ):ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var res =await service.GetAll();
            return Ok(res);
        }

        [HttpGet("{id}")]
        public async  Task<IActionResult> GetProductById(int id)
        {
            var res = await service.GetById(id);
            return Ok(res);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateProduct([FromForm] ProductDto product)
        {
            if(User.FindFirstValue(claimType:ClaimTypes.Role) != "Admin")
            {
                throw new UnauthorizedAccessException();
            }
            var res= await service.Create(product);
            return Ok(res);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductDto product)
        {
            if (User.FindFirstValue(claimType: ClaimTypes.Role) != "Admin")
            {
                throw new UnauthorizedAccessException();
            }
            var res= await service.Update(product, id);
            return Ok(res);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (User.FindFirstValue(claimType: ClaimTypes.Role) != "Admin")
            {
                throw new UnauthorizedAccessException();
            }
            var res= await service.DeleteProduct(id);
            return Ok(res);
        }

    }
}
