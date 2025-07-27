using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.OrderDto
{
    public class CreateOrder
    {

        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;

        public ICollection<OrderItemDto> OrderItems { get; set; } = [];

        public string PaymentMethod { get; set; } = string.Empty;


        public string Address { get; set; } = string.Empty; // Address for delivery or billing

        public int CustomerId { get; set; }

    }
    
}
