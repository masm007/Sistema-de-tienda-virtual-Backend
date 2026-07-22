using Application.DTOs.Images;
using Application.DTOs.Products;
using Domain.Entity;
using Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Product {
    public class GetAllActiveProductsUseCase {
        private IProductRepository<ProductEntity, int> _repository;

        public GetAllActiveProductsUseCase(IProductRepository<ProductEntity, int> repository) {
            _repository = repository;
        }

        public async Task<IEnumerable<ProductDto>> ExecuteAsync() {
            var products = await _repository.GetAllActiveAsync();
            if (products == null) {
                throw new InvalidOperationException("Producto no encontrado");
            }
            var response = new List<ProductDto>();
            foreach (var prd in products) {
                var images = new List<ProductImageDto>();
                foreach (var img in prd.Images) {
                    images.Add(new ProductImageDto(img.ImageUrl));
                }
                response.Add(new ProductDto(prd.Id, prd.Name, prd.Description, prd.CategoryId,
                prd.Price, prd.Sku, prd.Quantity, prd.IsAvailable, prd.IsActive, images));
            }
            return response;
        }
    }
}
