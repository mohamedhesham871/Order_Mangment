using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Dtos.OrderDto;

namespace Shared.Dtos.Customer
{
    public  class CustomerResultDto
    {

        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public string UserId { get; set; } = string.Empty; // Foreign key to the AppUser table

        public IEnumerable<OrderDtos> Orders { get; set; } = [];
    }
}
