using Application.DTOs.Images;
using Application.DTOs.Products;
using Application.DTOs.Users;
using Application.Interfaces.Storage;
using Domain.Entity;
using Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Product {
    public class GetProductByIdUseCase {
        private IProductRepository<ProductEntity, int> _repository;
        private readonly IProductImageRepository<ProductImageEntity, int> _imageRepository;

        public GetProductByIdUseCase(IProductRepository<ProductEntity, int> repository,
            IProductImageRepository<ProductImageEntity, int> imageRepository) {
            _repository = repository;
            _imageRepository = imageRepository;
        }

        public async Task<ProductDto?> ExecuteAsync(int id) {
            var prd = await _repository.GetByIdAsync(id);
            if (prd == null) {
                throw new InvalidOperationException("Producto no encontrado");
            }
            _imageRepository.
            var images = new List<ProductImageDto>();
            foreach (var item in prd.Images) {
                images.Add(new ProductImageDto(item.ImageUrl));
            }
            var response = new ProductDto(prd.Id, prd.Name,prd.Description, prd.CategoryId,
                prd.Price, prd.Sku, prd.Quantity, prd.IsAvailable, prd.IsActive, images);
            return response;
        }

    }
}
