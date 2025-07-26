using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domin.Models
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }

        public int OrderId { get; set; }   // This is the foreign key to the Order table
        public int ProductId { get; set; } // This is the foreign key to the Product table

        public ICollection<Product> Products { get; set; } = []; // navigation property to Product



    }
}
