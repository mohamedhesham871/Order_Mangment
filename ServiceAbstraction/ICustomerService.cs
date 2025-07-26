using Shared.Dtos.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstraction
{
    public interface ICustomerService
    {
        Task<CustomerResultDto> CreateCustomer(CreateCustomerDto createCustomer, string UserId);
        Task<CustomerResultDto> GetOrderOfCustomer(int id);
    }
}
