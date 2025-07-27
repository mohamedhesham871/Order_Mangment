using Domin.Contract;
using Domin.Exceptions;
using Domin.Models;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using ServiceAbstraction;
using Shared.Dtos.OrderDto;
using Stripe;
using Stripe.Climate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static service.PaymentService;

namespace service
{
    public  class PaymentService(IUnitOfWork unitOfWork,IConfiguration configuration ):IPaymentservice
    {
        //public class PaymentService(IBasketRepo basketRepo, IUnitOfwork unitOfwork, IMapper mapper, IConfiguration configuration)

            public async Task<OrderDtos> CreateOrUpdatePaymentIntent(int OrderId)

            {
                var Order = await unitOfWork.GetRepository<Domin.Models.Order>().GetByIdAsync(OrderId);
                if (Order is null) throw new NotFoundException("Order With this ID Not Found");

              
                
                // call Secret key to Config with Stripe Site
                StripeConfiguration.ApiKey = configuration["StripeSettings:SecretKey"];
                var Service = new PaymentIntentService();

                if (string.IsNullOrEmpty(Order.PaymentIntentId))
                {
                    //Create Payment Intent
                    var options = new PaymentIntentCreateOptions
                    {
                        Amount = (long)(Order.TotalAmount * 100),
                        Currency = "USD", // Change to your desired currency
                        PaymentMethodTypes = new List<string>
                                                                {   "card",
                                                                    "paypal",
                                                                },

                    };

                    var paymentIntent = await Service.CreateAsync(options);
                    Order.PaymentIntentId = paymentIntent.Id;
                }
                else
                {
                    //Update Payment Intent if needed (not implemented in this example)
                    var UpdateOptions = new PaymentIntentUpdateOptions
                    {
                        Amount = (long)(Order.TotalAmount * 100), // Stripe expects amount in cents
                    };

                    await Service.UpdateAsync(Order.PaymentIntentId, UpdateOptions);
                }

                 unitOfWork.GetRepository<Domin.Models.Order>().Update(Order);
                   var res = unitOfWork.SaveChanges();
                if (res <= 0)
                {
                    throw new BadResquestException("Error Occurred ");


                }

            var OrderRes = new OrderDtos()
            {
                OrderDate = Order.OrderDate,
                TotalAmount = Order.TotalAmount,
                OrderItems = Order.OrderItems.Select(item => new OrderItemDto
                {
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    Discount = item.Discount,
                    ProductId = item.ProductId
                }).ToList(),
                PaymentMethod = Order.PaymentMethod,
                Status = Order.Status.ToString(),
                Address = Order.Address,
                PaymentIntentId = Order.PaymentIntentId,
                CustomerId = Order.CustomerId
            };
            return OrderRes;

        }


    }
}
