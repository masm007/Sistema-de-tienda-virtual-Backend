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
    public class CouponRepository : ICouponRepository<CouponEntity, int> {
        private readonly ApplicationDbContext _context;

        public CouponRepository(ApplicationDbContext dbContext) {
            _context = dbContext;
        }

        public async Task CreateAsync(CouponEntity entity) {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            await _context.Coupons.AddAsync(entity);
        }

        public async Task<CouponEntity?> GetByCodeAsync(string code) {
            return await _context.Coupons
                .Include(c => c.RequiredProducts).ThenInclude(rp => rp.Product)
                .FirstOrDefaultAsync(c => c.Code == code);
        }

        public async Task<CouponEntity?> GetByIdAsync(int id) {
            return await _context.Coupons
                .Include(c => c.RequiredProducts).ThenInclude(rp => rp.Product)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<CouponEntity>> GetAllAsync() {
            return await _context.Coupons.AsNoTracking()
                .Include(c => c.RequiredProducts).ThenInclude(rp => rp.Product)
                .OrderBy(c => c.Id).ToListAsync();
        }

        public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();

        public Task UpdateAsync(CouponEntity entity) {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            _context.Coupons.Update(entity);
            return Task.CompletedTask;
        }
    }
}
