using Data.Persistence;
using Domain.Entity;
using Domain.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories {
    public class CategoryRepository : ICategoryRepository<CategoryEntity, int> {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext dbContext) {
            _context = dbContext;
        }

        public async Task CreateAsync(CategoryEntity entity) {
            if (entity == null) {
                throw new ArgumentNullException(nameof(entity));
            }
            await _context.Categories.AddAsync(entity);
        }

        public Task DeleteAsync(CategoryEntity entity) {
            if (entity == null) {
                throw new ArgumentNullException(nameof(entity));
            }
            _context.Categories.Remove(entity);
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<CategoryEntity>> GetAllAsync() {
            return await _context.Categories.AsNoTracking().OrderBy(cat => cat.Id).ToListAsync();
        }

        public async Task<CategoryEntity?> GetByIdAsync(int id) {
            return await _context.Categories.FirstOrDefaultAsync(cat => cat.Id == id);
        }

        public async Task<int> SaveChangesAsync() {
            return await _context.SaveChangesAsync();
        }

        public Task UpdateAsync(CategoryEntity entity) {
            if (entity == null) {
                throw new ArgumentNullException(nameof(entity));
            }
            _context.Categories.Update(entity);
            return Task.CompletedTask;
        }
    }
}
