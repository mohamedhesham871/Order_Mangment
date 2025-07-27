using Domin.Contract;
using Domin.Exceptions;
using Domin.Models;
using Domin.Models.identitys;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceAbstraction;
using Shared.Dtos;
using Shared.Dtos.Customer;
using Shared.Dtos.OrderDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace service
{
    public class CustomerServices(IUnitOfWork unitOfWork):ICustomerService
    {
        public async Task<CustomerResultDto> CreateCustomer([FromForm]CreateCustomerDto createCustomer,string UserId)
        {
            if(createCustomer == null) {
                throw new BadResquestException( "Customer data cannot be null");
            }
            var res = await unitOfWork.GetRepository<Customer>().GetByIdAsync(createCustomer.Email);
            if (res is not null)
                throw new BadResquestException("This Email IS Already Exist, Please Try Another Email.");

            //convert CreateCustomerDto to Customer entity
            var customer = new Customer
            {
                Email = createCustomer.Email,
                Name = createCustomer.Name,
                UserId = UserId //user that Create Customer 
            };
             unitOfWork.GetRepository<Customer>().Add(customer);
            var result = unitOfWork.SaveChanges();
            if (result <= 0)
            {
                throw new BadResquestException("Failed to create customer");
            }
            return new CustomerResultDto
            {
                Id = customer.Id,
                Name = customer.Name,
                Email = customer.Email,
                UserId = customer.UserId
            };
        }

       

        public async Task<CustomerResultDto> GetOrderOfCustomer(int id)
        {
            var customer = await unitOfWork.GetRepository<Customer>().GetByIdAsync(id);
            if (customer == null)
            {
                throw new NotFoundException("Customer not found");
            }

            var ordersOfCustomer = await unitOfWork.GetRepository<Order>()
                .GetAllIncludingAsync(o => o.CustomerId == id,
                    include => include.Include(o => o.OrderItems));

            var ordersDto = ordersOfCustomer.Select(order => new OrderDtos
            {
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                PaymentMethod = order.PaymentMethod,
                Status = order.Status.ToString(),
                Address = order.Address,
                PaymentIntentId = order.PaymentIntentId,
                CustomerId = order.CustomerId,

                OrderItems = order.OrderItems.Select(item => new OrderItemDto
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    Discount = item.Discount
                }).ToList()
            }).ToList();

            return new CustomerResultDto
            {
                Id = customer.Id,
                Name = customer.Name,
                Email = customer.Email,
                UserId = customer.UserId,
                Orders = ordersDto
            };
        }



    }
}
