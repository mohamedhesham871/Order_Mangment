using Shared;
using Shared.Dtos.OrderDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstraction
{
    public  interface IOrderServices
    {
        Task<OrderDtos> CreateOrder(CreateOrder order);
        Task<OrderDtos> GetOrderById(int id);
        Task<IEnumerable<OrderDtos>> GetAllOrders();
        Task UpdateOrderStatus(int id , OrderPaymentStatusDto Status);

    }
}
