using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Identity.Client;
using ServiceAbstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class InvoiceController(IInvoiceService service) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetALlInvoice()
        {
            var res = await service.GetAllInVoicesAsync();
            return Ok(res);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetInvoice(int id)
        {
            var res = await service.GetInVoicesByIdAsync(id);
            return Ok(res);
        }

    }
}
