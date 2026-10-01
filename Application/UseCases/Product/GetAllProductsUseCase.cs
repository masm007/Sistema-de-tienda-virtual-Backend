using Application.DTOs.Categories;
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
    public class GetAllProductsUseCase {
        private IProductRepository<ProductEntity, int> _repository;

        public GetAllProductsUseCase(IProductRepository<ProductEntity, int> repository) {
            _repository = repository;
        }

        public async Task<IEnumerable<ProductDto>> ExecuteAsync() {
            var products = await _repository.GetAllAsync();
            if (products == null) {
                throw new InvalidOperationException("No hay ningun producto");
            }
            var response = new List<ProductDto>();
            foreach (var prd in products) {
                var images = new List<ProductImageDto>();
                foreach (var img in prd.Images) {
                    images.Add(new ProductImageDto(img.Id, img.ImageUrl));
                }
                var category = new CategorySummaryDto(prd.Category.Id, prd.Category.Name);
                response.Add(new ProductDto(prd.Id, prd.Name, prd.Sku, prd.Description, category,
                prd.Price, prd.Quantity, prd.IsAvailable, prd.IsActive, images));
            }
            return response;
        }
    }
}
