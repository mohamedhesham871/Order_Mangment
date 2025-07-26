using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos
{
    public class OrderDto
    {
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;

        public decimal TotalAmount { get; set; }

        public ICollection<OrderItemDto> OrderItems { get; set; } = [];

        public string PaymentMethod { get; set; } = string.Empty;

        public string Status { get; set; } 

        public string Address { get; set; } = string.Empty;

        public string? PaymentIntentId { get; set; }

        public int CustomerId { get; set; } 

    }
}
