using Application.DTOs.Categories;
using Application.Interfaces.Storage;
using Domain.Entity;
using Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Category {
    public class CreateCategoryUseCase {
        private readonly ICategoryRepository<CategoryEntity, int> _categoryRepository;

        public CreateCategoryUseCase(ICategoryRepository<CategoryEntity, int> categoryRepository) {
            _categoryRepository = categoryRepository;
        }

        public async Task<CategoryDto> ExecuteAsync(CreateCategoryDto dto) {
            if (dto == null) {
                throw new ArgumentNullException(nameof(dto));
            }
            var category = new CategoryEntity(dto.Name, dto.Description);
            await _categoryRepository.CreateAsync(category);
            await _categoryRepository.SaveChangesAsync();

            return new CategoryDto(category.Id, category.Name, category.Description);
        }
    }
}
