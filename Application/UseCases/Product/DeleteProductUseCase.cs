using Application.DTOs.Products;
using Application.Interfaces.Storage;
using Domain.Entity;
using Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Product {
    public class DeleteProductUseCase {
        private IProductRepository<ProductEntity, int> _repository;
        private readonly IImageStorageService _imageStorageService;

        public DeleteProductUseCase(IProductRepository<ProductEntity, int> repository) {
            _repository = repository;
        }

        public async Task ExecuteAsync(CreateProductDto dto) {
            foreach (var img in dto.Images) {
                try {
                    var imagen = await _imageStorageService.DeleteImageAsync(img.);
                } catch (Exception ex) { 

                }
            }
            await _repository.DeleteAsync(new ProductEntity(dto.Name, dto.Description, dto.Price, dto.Quantity,
                dto.Images, dto.Sku, dto.CategoryId));
            await _repository.SaveChangesAsync();
        }
    }
}
