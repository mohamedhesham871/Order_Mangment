using Microsoft.AspNetCore.Mvc;
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
    public  class PaymentController(IPaymentservice services):ControllerBase
    {
        [HttpPost("{OrderID}")]
        public async Task<IActionResult> Payment(int OrderID)
        {
            var res =await  services.CreateOrUpdatePaymentIntent(OrderID);
            return Ok(res);
        }
    }
}
