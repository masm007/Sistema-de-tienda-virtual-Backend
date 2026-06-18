using Application.DTOs.Categories;
using Application.DTOs.Products;
using Domain.Entity;
using Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Category {
    public class GetAllCategoriesUseCase {
        private readonly ICategoryRepository<CategoryEntity, int> _categoryRepository;

        public GetAllCategoriesUseCase(ICategoryRepository<CategoryEntity, int> categoryRepository) {
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<CategoryDto>> ExecuteAsync() {
            var categories = await _categoryRepository.GetAllAsync();
            if (categories == null) {
                throw new InvalidOperationException("No hay ninguna categoria");
            }
            var response = new List<CategoryDto>();
            foreach (var cat in categories) {
                response.Add(new CategoryDto(cat.Id, cat.Name, cat.Description));
            }
            return response;
        }
    }
}
