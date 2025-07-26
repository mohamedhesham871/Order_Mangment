using Domin;
using Domin.Contract;
using Domin.Exceptions;
using ServiceAbstraction;
using Shared.Dtos.ProductDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace service
{
    public class ProductServices(IUnitOfWork unitOfWork) : IProductService
    {
        public async  Task<ProductDto> Create(ProductDto product)
        {
            //convert From Dto to Product model 
            if(product.Price<0 || product.Stock < 0 || string.IsNullOrEmpty(product.Name))
                throw new BadResquestException("Invalid product data");

            var productModel = new Product()
            {
                Price = product.Price,
                Name = product.Name,
                Description = product.Description,
                Stock = product.Stock
            };
            unitOfWork.GetRepository<Product>().Add(productModel);
            var result =  unitOfWork.SaveChanges();
            if (result <= 0)
                throw new BadResquestException("Failed to create product");
            
            return product;
        }

        public  async Task<bool> DeleteProduct(int id)
        {
            var Product = await unitOfWork.GetRepository<Product>().GetByIdAsync(id);
            if (Product == null)
                throw new NotFoundException("Product not found");
            unitOfWork.GetRepository<Product>().Delete(Product);
            var res =unitOfWork.SaveChanges();
            if (res <= 0)
                throw new BadResquestException("Failed to delete product");

            return true;
        }

        public async Task<IEnumerable<ProductDto>> GetAll()
        {
            var products = await unitOfWork.GetRepository<Product>().GetAllAsync();

            //convert it to IEnumrable<ProductDto>
            var productDtos = products.Select(p => new ProductDto
            {

                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                Stock = p.Stock
            });

            return productDtos;
        }

        public async Task<ProductDto> GetById(int id)
        {
           var product = await unitOfWork.GetRepository<Product>().GetByIdAsync(id);
            if (product == null)
                throw new NotFoundException("Product not found");

            //convert it to ProductDto
            var productDto = new ProductDto
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock
            };

            return productDto;
        }

        public async Task<ProductDto> Update(ProductDto product, int id)
        {
            var ProductModel =await unitOfWork.GetRepository<Product>().GetByIdAsync(id);
            if (ProductModel == null)
                throw new NotFoundException("Product not found");
            if (product.Price < 0 || product.Stock < 0 || string.IsNullOrEmpty(product.Name))
                throw new BadResquestException("Invalid product data");
            #region Change Data

            ProductModel.Name = product.Name;
            ProductModel.Description = product.Description;
            ProductModel.Price = product.Price;
            ProductModel.Stock = product.Stock;

            #endregion
            var res= unitOfWork.SaveChanges();
            if (res <= 0)
                throw new BadResquestException("Failed to update product");
            return product;

        }
    }
}
