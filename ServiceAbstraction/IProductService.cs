using Shared.Dtos.ProductDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstraction
{
    public interface IProductService
    {
        Task<ProductDto> GetById(int id);
        Task<IEnumerable<ProductDto>> GetAll();
        Task<ProductDto> Create(ProductDto product);
        Task<ProductDto> Update(ProductDto product,int id);
        Task<bool> DeleteProduct(int id);
    }
}
