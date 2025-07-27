using Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstraction
{
    public  interface IInvoiceService
    {
        Task<InVoicesDto> GetInVoicesByIdAsync(int id );
        Task<IEnumerable<InVoicesDto>> GetAllInVoicesAsync();

    }
}
