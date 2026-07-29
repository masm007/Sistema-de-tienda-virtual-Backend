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
        private readonly IProductRepository<ProductEntity, int> _productRepository;
        //repository de img
        private readonly IImageStorageService _imageStorageService;

        public CreateProductUseCase(IProductRepository<ProductEntity, int> repository,
            IImageStorageService imageStorageService, ICategoryRepository<CategoryEntity, int> categoryRepository) {
            _productRepository = repository;
            _categoryRepository = categoryRepository;
            _imageStorageService = imageStorageService;
        }

        public async Task<CreateProductResponseDto> ExecuteAsync(CreateProductDto dto) {
            if (dto == null) {
                throw new ArgumentNullException(nameof(dto));
            }
            if (dto.Images == null || dto.Images.Count == 0) {
                throw new ArgumentException("Debe enviar al menos una imagen");
            }
            var category = await _categoryRepository.GetByIdAsync(dto.CategoryId);
            if (category == null) {
                throw new InvalidOperationException("La categoría no existe");
            }
            var imageProducts = new List<ProductImageEntity>();
            // PublicIds de las imágenes que sí lograron subirse
            var uploadedPublicIds = new List<string>();
            try {
                foreach (var img in dto.Images) {
                    using var stream = img.FileStream;
                    var uploaded = await _imageStorageService.UploadImageAsync(stream, img.FileName);
                    uploadedPublicIds.Add(uploaded.PublicId);
                    imageProducts.Add(new ProductImageEntity(uploaded.PublicId, uploaded.Url));
                }
                var product = new ProductEntity(dto.Name, dto.Description, dto.Price, dto.Quantity,
                imageProducts, dto.Sku, dto.CategoryId);
                await _productRepository.CreateAsync(product);
                //no es necesario llamar al repositorio de la tabla de imagenes
                await _productRepository.SaveChangesAsync();
                //ef core rastrea el id y lo asigna por asi decirlo de esta manera no es 0
                return new CreateProductResponseDto(product.Id,product.Name);
            } catch {
                // Eliminar las imágenes que ya se habían subido
                foreach (var publicId in uploadedPublicIds) {
                    try {
                        await _imageStorageService.DeleteImageAsync(publicId);
                    } catch {
                        // Se ignora la excepción para no ocultar la causa original del fallo
                    }
                }
                throw;
            }
        }

    }
}
