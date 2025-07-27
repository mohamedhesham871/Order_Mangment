using Api.Emailsettings;
using Domin;
using Domin.Contract;
using Domin.Exceptions;
using Domin.Models;
using Domin.Models.identitys;
using ServiceAbstraction;
using Shared;
using Shared.Dtos.OrderDto;
using Stripe.Climate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Products = Domin.Product;

namespace service
{
    public class OrderService(IUnitOfWork unitOfWork) : IOrderServices
    {
        public async Task<IEnumerable<OrderDtos>> GetAllOrders()//Only Admins
        {
            var res = await unitOfWork.GetRepository<Domin.Models.Order>().GetAllAsync();
            var Orders = res.Select(order => new OrderDtos
            {
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                OrderItems = order.OrderItems.Select(item => new OrderItemDto
                {
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    Discount = item.Discount,
                    ProductId = item.ProductId
                }).ToList(),
                PaymentMethod = order.PaymentMethod,
                Status = order.Status.ToString(),
                Address = order.Address,
                PaymentIntentId = order.PaymentIntentId,
                CustomerId = order.CustomerId
            });
            return Orders;
        }

        public async Task<OrderDtos> GetOrderById(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid Order ID");
            var order = await unitOfWork.GetRepository<Domin.Models.Order>().GetByIdAsync(id);
            if (order == null)
                throw new NotFoundException("Order not found");
            var orderDto = new OrderDtos
            {
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                OrderItems = order.OrderItems.Select(item => new OrderItemDto
                {
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    Discount = item.Discount,
                    ProductId = item.ProductId
                }).ToList(),
                PaymentMethod = order.PaymentMethod,
                Status = order.Status.ToString(),
                Address = order.Address,
                PaymentIntentId = order.PaymentIntentId,
                CustomerId = order.CustomerId
            };
            return orderDto;
        }
       
        public async Task UpdateOrderStatus(int id , OrderPaymentStatusDto status)
        {
            var orderRepo = unitOfWork.GetRepository<Domin.Models.Order>();
            var order = await orderRepo.GetByIdAsync(id);

            if (order == null)
                throw new NotFoundException($"Order with ID {id} not found.");

            order.Status = Enum.Parse<OrderPaymentStatus>(status.ToString(),true);
            foreach (var item in order.OrderItems)
            {
                var prodcuts = await unitOfWork.GetRepository<Products>().GetByIdAsync(item.ProductId);
                if (prodcuts is null)
                {
                    throw new NotFoundException("Product Not Found");
                }
                if (prodcuts.Stock - item.Quantity <= 0)
                {
                    throw new BadResquestException($"Out Of Stock Of Product {prodcuts.Id}");
                }
            }
            unitOfWork.GetRepository<Domin.Models.Order>().Update(order);
            var Invoice = new Invoice()
            {
                OrderId = order.Id,
                InvoiceDate = DateTimeOffset.Now,
                TotalAmount = order.TotalAmount
            };
           
            unitOfWork.GetRepository<Invoice>().Update(Invoice);
            var res = unitOfWork.SaveChanges();
            if(res<=0)throw new BadResquestException("Error When Update Order Status");
            
        }
        public async Task<OrderDtos> CreateOrder(CreateOrder order)
        {
            if (order == null)
            {
                throw new ArgumentNullException("Can't Be Null Data");
            }

            // Map CreateOrder to Order entity
            var newOrder = new Domin.Models.Order
            {
                OrderDate = DateTimeOffset.Now,
                PaymentMethod = order.PaymentMethod,
                Address = order.Address,
                Status = OrderPaymentStatus.Pending, // Default status
                CustomerId = order.CustomerId,
                OrderItems = order.OrderItems.Select(item => new OrderItem
                {
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    Discount = item.Discount,
                    ProductId = item.ProductId
                }).ToList()
            };

            foreach (var item in order.OrderItems)
            {
                var prodcuts = await unitOfWork.GetRepository<Products>().GetByIdAsync(item.ProductId);
                if (prodcuts is null)
                {
                    throw new NotFoundException("Product Not Found");
                }
                if (prodcuts.Stock - item.Quantity <= 0)
                {
                    throw new BadResquestException($"Out Of Stock Of Product {prodcuts.Id}");
                }
            }
                #region Discount
                var subtotal = order.OrderItems.Sum(x =>
                    x.UnitPrice * x.Quantity * (1 - x.Discount));
                //Apply tiered order-level discount
                decimal orderLevelDiscount = 0m;
                if (subtotal > 200) orderLevelDiscount = 0.10m;
                else if (subtotal > 100) orderLevelDiscount = 0.05m;

                var discountedTotal = subtotal * (1 - orderLevelDiscount);
                newOrder.TotalAmount = Math.Round(discountedTotal, 2);

                #endregion
                unitOfWork.GetRepository<Domin.Models.Order>().Add(newOrder);
            #region Create Invoice 

            var Invoice = new Invoice()
            {
                OrderId = newOrder.Id,
                InvoiceDate = DateTimeOffset.Now,
                TotalAmount = newOrder.TotalAmount
            };
            unitOfWork.GetRepository<Invoice>().Add(Invoice);
            #endregion
            var res = unitOfWork.SaveChanges();
                if (res < 0)
                {
                    throw new BadResquestException("Error When Create Order");
                }

            #region Sending Email
            var customerId = newOrder.CustomerId;
            var customer = await unitOfWork.GetRepository<Customer>().GetByIdAsync(customerId);
            if (customer is null)
                throw new NotFoundException($"{nameof(Customer)}: {customerId}");
            await SendEmail.SendingMessage(customer.Name, newOrder.Id, newOrder.Status.ToString());

            #endregion // convert the Order entity to OrderDto
            var OrderRes = new OrderDtos()
                {
                    OrderDate = newOrder.OrderDate,
                    TotalAmount = newOrder.TotalAmount,
                    OrderItems = newOrder.OrderItems.Select(item => new OrderItemDto
                    {
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice,
                        Discount = item.Discount,
                        ProductId = item.ProductId
                    }).ToList(),
                    PaymentMethod = newOrder.PaymentMethod,
                    Status = newOrder.Status.ToString(),
                    Address = newOrder.Address,
                    PaymentIntentId = newOrder.PaymentIntentId,
                    CustomerId = newOrder.CustomerId
                };

                return OrderRes;
            }
        }
}
