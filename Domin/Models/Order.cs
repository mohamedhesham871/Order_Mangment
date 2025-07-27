using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domin.Models
{
    public class Order
    {
        public int Id { get; set; }
        
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
        
        public decimal TotalAmount { get; set; }
        
        public ICollection<OrderItem> OrderItems { get; set; } = [];
        
        public string PaymentMethod { get; set; } = string.Empty;
        
        public OrderPaymentStatus Status { get; set; } = OrderPaymentStatus.Pending;
        
        public string Address { get; set; } = string.Empty; // Address for delivery or billing
        
        public string ?PaymentIntentId { get; set; }  
        
        public int CustomerId { get; set; } // Foreign key to the Customer table

    }
}
