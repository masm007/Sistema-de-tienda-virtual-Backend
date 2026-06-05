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
    public class ProductImageRepository : IProductImageRepository<ProductImageEntity, int> {
        private readonly ApplicationDbContext _context;

        public ProductImageRepository(ApplicationDbContext dbContext) {
            _context = dbContext;
        }

        public async Task<IEnumerable<ProductImageEntity>> GetAllAsync() {
            return await _context.ProductImages.AsNoTracking().OrderBy(img => img.Id).ToListAsync();
        }

        public async Task<IEnumerable<ProductImageEntity>> GetAllByProductIdAsync(int productId) {
            return await _context.ProductImages.Where(img => img.ProductId == productId)
                .OrderBy(prd => prd.Id).ToListAsync();
        }

        public async Task<ProductImageEntity?> GetByIdAsync(int id) {
            return await _context.ProductImages.FirstOrDefaultAsync(img => img.Id == id);
        }

        public async Task CreateAsync(ProductImageEntity entity) {
            if (entity == null) {
                throw new ArgumentNullException(nameof(entity));
            }
            await _context.ProductImages.AddAsync(entity);
        }

        public async Task DeleteAllByProductIdAsync(int productId) {
            await _context.ProductImages.Where(x => x.ProductId == productId).ExecuteDeleteAsync();
        }

        public Task DeleteAsync(ProductImageEntity entity) {
            if (entity == null) {
                throw new ArgumentNullException(nameof(entity));
            }
            _context.ProductImages.Remove(entity);
            return Task.CompletedTask;
        }

        public async Task<int> SaveChangesAsync() {
            return await _context.SaveChangesAsync();
        }

        //no sera implementado por ahora
        public Task UpdateAsync(ProductImageEntity entity) {
            throw new NotImplementedException();
        }
    }
}
