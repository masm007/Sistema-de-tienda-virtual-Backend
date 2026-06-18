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

        public DeleteCategoryUseCase(ICategoryRepository<CategoryEntity, int> categoryRepository) {
            _categoryRepository = categoryRepository;
        }

        public async Task ExecuteAsync(int id) {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null) {
                throw new InvalidOperationException("Categoria con ese Id no encontrada");
            }
            await _categoryRepository.DeleteAsync(category);
            await _categoryRepository.SaveChangesAsync();
        }
    }
}
