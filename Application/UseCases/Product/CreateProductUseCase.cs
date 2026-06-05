using Application.DTOs.Images;
using Application.DTOs.Products;
using Application.DTOs.User;
using Application.Interfaces.Security;
using Application.Interfaces.Storage;
using Domain.Entity;
using Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Product {
    public class CreateProductUseCase {
        private readonly ICategoryRepository<CategoryEntity, int> _categoryRepository;
        private readonly IProductRepository<ProductEntity, int> _repository;
        //repository de img
        private readonly IImageStorageService _imageStorageService;

        public CreateProductUseCase(IProductRepository<ProductEntity, int> repository,
            IImageStorageService imageStorageService, ICategoryRepository<CategoryEntity, int> categoryRepository) {
            _repository = repository;
            _categoryRepository = categoryRepository;
            _imageStorageService = imageStorageService;
        }

        public async Task<ProductResponseDto> ExecuteAsync(CreateProductDto dto) {
            if (dto == null) {
                throw new ArgumentNullException(nameof(dto));
            }
            if (dto.Images == null || dto.Images.Count == 0) {
                throw new ArgumentException("Debe enviar al menos una imagen");
            }
            var category = await _categoryRepository.GetByIdAsync(dto.CategoryId);
            if (category == null) {
                throw new ArgumentException("La categoría no existe");
            }
            //a futuro validar que se suban todas las imagenes
            //try catch()
            var imageProducts = new List<ProductImageEntity>();
            foreach (var img in dto.Images) {
                var imagen = await _imageStorageService.UploadImageAsync(img.FileStream, img.FileName);
                imageProducts.Add(new ProductImageEntity(imagen.PublicId, imagen.Url));
            }
            var product = new ProductEntity(dto.Name, dto.Description, dto.Price, dto.Quantity, 
                imageProducts, dto.Sku, dto.CategoryId);
            await _repository.CreateAsync(product);
            await _repository.SaveChangesAsync();

            return new ProductResponseDto(product.Id, product.Name);
        }

    }
}
