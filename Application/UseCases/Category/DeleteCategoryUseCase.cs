using Application.Interfaces.Storage;
using Domain.Entity;
using Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Category {
    public class DeleteCategoryUseCase {
        private readonly ICategoryRepository<CategoryEntity, int> _categoryRepository;
        private readonly IProductRepository<ProductEntity, int> _productRepository;

        public DeleteCategoryUseCase(ICategoryRepository<CategoryEntity, int> categoryRepository,
            IProductRepository<ProductEntity, int> productRepository) {
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
        }

        public async Task ExecuteAsync(int id) {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null) {
                throw new InvalidOperationException("Categoria con ese Id no encontrada");
            }
            // Se valida antes de borrar en vez de dejar que falle la restricción de llave
            // foránea en la base de datos, para devolver un mensaje claro al cliente.
            var products = await _productRepository.GetAllByCategoryIdAsync(id);
            if (products.Any()) {
                throw new ArgumentException(
                    "No se puede eliminar la categoría porque tiene productos asociados.");
            }
            await _categoryRepository.DeleteAsync(category);
            await _categoryRepository.SaveChangesAsync();
        }
    }
}
