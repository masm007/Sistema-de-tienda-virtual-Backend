using Application.DTOs.Products;
using Application.Interfaces.Storage;
using Domain.Entity;
using Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.UseCases.Product {
    public class UpdateProductUseCase {
        private readonly IProductRepository<ProductEntity, int> _repository;
        private readonly ICategoryRepository<CategoryEntity, int> _categoryRepository;
        private readonly IImageStorageService _imageStorageService;

        public UpdateProductUseCase(IProductRepository<ProductEntity, int> repository,
            IImageStorageService imageStorageService, ICategoryRepository<CategoryEntity, int> categoryRepository) {
            _repository = repository;
            _categoryRepository = categoryRepository;
            _imageStorageService = imageStorageService;
        }

        public async Task<CreateProductResponseDto> ExecuteAsync(UpdateProductDto dto) {
            if (dto == null) {
                throw new ArgumentNullException(nameof(dto));
            }
            var prd = await _repository.GetByIdAsync(dto.Id);
            if (prd == null) {
                throw new InvalidOperationException("Producto no encontrado");
            }
            var category = await _categoryRepository.GetByIdAsync(dto.CategoryId);
            if (category == null) {
                throw new InvalidOperationException("La categoría no existe");
            }

            var keptImages = prd.Images.Where(img => dto.KeepImageIds.Contains(img.Id)).ToList();
            var removedImages = prd.Images.Where(img => !dto.KeepImageIds.Contains(img.Id)).ToList();

            var newImageEntities = new List<ProductImageEntity>();
            // PublicIds de las imágenes nuevas que sí lograron subirse
            var uploadedPublicIds = new List<string>();
            try {
                foreach (var img in dto.NewImages) {
                    using var stream = img.FileStream;
                    var uploaded = await _imageStorageService.UploadImageAsync(stream, img.FileName);
                    uploadedPublicIds.Add(uploaded.PublicId);
                    newImageEntities.Add(new ProductImageEntity(uploaded.PublicId, uploaded.Url));
                }

                var finalImages = keptImages.Concat(newImageEntities).ToList();
                prd.UpdateInfo(dto.Name, dto.Description, dto.Price, dto.CategoryId, dto.Quantity,
                    dto.IsAvailable, dto.IsActive, finalImages, dto.Sku);

                await _repository.UpdateAsync(prd);
                await _repository.SaveChangesAsync();
            } catch {
                // Eliminar las imágenes nuevas que ya se habían subido
                foreach (var publicId in uploadedPublicIds) {
                    try {
                        await _imageStorageService.DeleteImageAsync(publicId);
                    } catch {
                        // Se ignora la excepción para no ocultar la causa original del fallo
                    }
                }
                throw;
            }

            // Solo se eliminan de Cloudinary una vez que el guardado en base de datos tuvo éxito
            foreach (var removed in removedImages) {
                try {
                    await _imageStorageService.DeleteImageAsync(removed.CloudinaryPublicId);
                } catch {
                    // Se ignora: el producto ya quedó guardado correctamente
                }
            }

            return new CreateProductResponseDto(prd.Id, prd.Name);
        }
    }
}
