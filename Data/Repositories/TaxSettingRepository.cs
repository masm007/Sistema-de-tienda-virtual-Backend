using Data.Persistence;
using Domain.Entity;
using Domain.Repository;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Data.Repositories {
    public class TaxSettingRepository : ITaxSettingRepository {
        private readonly ApplicationDbContext _context;

        public TaxSettingRepository(ApplicationDbContext dbContext) {
            _context = dbContext;
        }

        public async Task<TaxSettingEntity?> GetAsync() {
            return await _context.TaxSettings.FirstOrDefaultAsync();
        }

        public Task UpdateAsync(TaxSettingEntity entity) {
            _context.TaxSettings.Update(entity);
            return Task.CompletedTask;
        }

        public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}
