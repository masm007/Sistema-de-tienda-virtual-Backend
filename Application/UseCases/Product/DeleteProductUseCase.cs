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

        public DeleteProductUseCase(IProductRepository<ProductEntity, int> repository,
            IImageStorageService imageStorageService) {
            _repository = repository;
            _imageStorageService = imageStorageService;
        }

        public async Task ExecuteAsync(int id) {
            var prd = await _repository.GetByIdAsync(id);
            if (prd == null) {
                throw new InvalidOperationException("Producto no encontrado");
            }
            foreach (var img in prd.Images) {
                await _imageStorageService.DeleteImageAsync(img.CloudinaryPublicId);
            }
            await _repository.DeleteAsync(prd);
            await _repository.SaveChangesAsync();
        }
    }
}
