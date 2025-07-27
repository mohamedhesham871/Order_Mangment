using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos
{
    public  class InVoicesDto
    {
        public int InvoiceId { get; set; }
        public int OrderId { get; set; } 
        public DateTimeOffset InvoiceDate { get; set; } = DateTimeOffset.Now;
        public decimal TotalAmount { get; set; }
    }
}
