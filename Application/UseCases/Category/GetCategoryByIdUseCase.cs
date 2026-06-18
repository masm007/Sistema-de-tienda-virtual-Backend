using Application.DTOs.Categories;
using Application.DTOs.Images;
using Application.DTOs.Products;
using Domain.Entity;
using Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Category {
    public class GetCategoryByIdUseCase {
        private readonly ICategoryRepository<CategoryEntity, int> _categoryRepository;

        public GetCategoryByIdUseCase(ICategoryRepository<CategoryEntity, int> categoryRepository) {
            _categoryRepository = categoryRepository;
        }

        public async Task<CategoryDto?> ExecuteAsync(int id) {
            var prd = await _categoryRepository.GetByIdAsync(id);
            if (prd == null) {
                throw new InvalidOperationException("Categoria con ese Id no encontrada");
            }
            var response = new CategoryDto(prd.Id, prd.Name, prd.Description);
            return response;
        }
    }
}
