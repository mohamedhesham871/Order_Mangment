using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domin.Models
{
    public class Invoice
    {
        public int InvoiceId { get; set; }
        public int OrderId { get; set; } // Foreign key to the Order table
        public DateTimeOffset InvoiceDate { get; set; }= DateTimeOffset.Now;
        public decimal TotalAmount { get; set; }
    }
}
