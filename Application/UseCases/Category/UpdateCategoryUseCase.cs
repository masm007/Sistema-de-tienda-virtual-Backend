using Application.DTOs.Categories;
using Application.Interfaces.Security;
using Domain.Entity;
using Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Category {
    public class UpdateCategoryUseCase {
        private readonly ICategoryRepository<CategoryEntity, int> _categoryRepository;

        public UpdateCategoryUseCase(ICategoryRepository<CategoryEntity, int> categoryRepository) {
            _categoryRepository = categoryRepository;
        }

        public async Task<CategoryDto?> ExecuteAsync(CategoryDto dto) {
            var prd = await _categoryRepository.GetByIdAsync(dto.Id);
            if (prd == null) {
                throw new InvalidOperationException("Categoria con ese Id no encontrada");
            }
            prd.UpdateInfo(dto.Name, dto.Description);
            await _categoryRepository.UpdateAsync(prd);
            await _categoryRepository.SaveChangesAsync();
            return new CategoryDto(prd.Id, prd.Name, prd.Description);
        }
    }
}
