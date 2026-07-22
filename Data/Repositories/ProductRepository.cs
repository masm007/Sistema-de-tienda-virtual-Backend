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
    public class ProductRepository : IProductRepository<ProductEntity, int> {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext dbContext) {
            _context = dbContext;
        }

        public async Task CreateAsync(ProductEntity entity) {
            if (entity == null) {
                throw new ArgumentNullException(nameof(entity));
            }
            //guardar las imagenes
            await _context.AddAsync(entity);
        }

        public Task DeleteAsync(ProductEntity entity) {
            if (entity == null) {
                throw new ArgumentNullException(nameof(entity));
            }
            _context.Products.Remove(entity);
            //eliminar las imagenes
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<ProductEntity>> GetAllActiveAsync() {
            return await _context.Products.AsNoTracking().Include(prd => prd.Images)
                .Where(prd => prd.IsActive).OrderBy(prd => prd.Id).ToListAsync();
        }

        public async Task<IEnumerable<ProductEntity>> GetAllAsync() {
            return await _context.Products.AsNoTracking().Include(prd => prd.Images)
                .OrderBy(prd => prd.Id).ToListAsync();
        }

        public async Task<IEnumerable<ProductEntity>> GetAllByCategoryIdAsync(int categoryId) {
            return await _context.Products.Where(prd => prd.CategoryId == categoryId)
                .OrderBy(prd => prd.Id).ToListAsync();
        }

        public async Task<ProductEntity?> GetByIdAsync(int id) {
            return await _context.Products.Include(p => p.Images).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<int> SaveChangesAsync() {
            return await _context.SaveChangesAsync();
        }

        public Task UpdateAsync(ProductEntity entity) {
            if (entity == null) {
                throw new ArgumentNullException(nameof(entity));
            }
            //guardar las imagenes
            _context.Products.Update(entity);
            return Task.CompletedTask;
        }
    }
}
