using Domin.Contract;
using Domin.Exceptions;
using ServiceAbstraction;
using Shared.Dtos;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using invoices = Domin.Models.Invoice;
namespace service
{
    public class InvoicesService(IUnitOfWork unitOfWork) : IInvoiceService
    {
        public async Task<IEnumerable<InVoicesDto>> GetAllInVoicesAsync()
        {
           var res  = await unitOfWork.GetRepository<invoices>().GetAllAsync();
            //convert to Dto
            var Dto = res.Select(res => new InVoicesDto
            {
                InvoiceDate = res.InvoiceDate,
                InvoiceId = res.InvoiceId,
                TotalAmount = res.TotalAmount,
                OrderId = res.OrderId
            });
               return Dto;
        }

        public async Task<InVoicesDto> GetInVoicesByIdAsync(int id)
        {
            if (id > 0)
            {
                throw new ArgumentException("Id is not Valid");
            }
            var Invoice= await unitOfWork.GetRepository<invoices>().GetByIdAsync(id);
            if (Invoice == null)
                throw new NotFoundException($"Invoice with is id : {id} not Found");
            var dto = new InVoicesDto()
            {
                //InvoiceDate = Invoice.,
                InvoiceDate = Invoice.InvoiceDate,
                InvoiceId = Invoice.InvoiceId,
                TotalAmount = Invoice.TotalAmount,
                OrderId = Invoice.OrderId
            };
            return dto;
        }
    }
}
