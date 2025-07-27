using Shared.Dtos.OrderDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstraction
{
   public  interface IPaymentservice
    {
        Task<OrderDtos> CreateOrUpdatePaymentIntent(int OrderId);
    }
}
