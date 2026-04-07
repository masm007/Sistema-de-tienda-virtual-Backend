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
    public class UserRepository : IRepository<UserEntity, int> {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext dbContext) {
            _context = dbContext;
        }
        public async Task CreateAsync(UserEntity entity) {
            if(entity == null) {
                throw new ArgumentNullException(nameof(entity)); 
            }
            await _context.AddAsync(entity);
        }

        public Task DeleteAsync(UserEntity entity) {
            if (entity == null) {
                throw new ArgumentNullException(nameof(entity));
            }
            _context.Users.Remove(entity);
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<UserEntity>> GetAllAsync() {
            //solo lectura para ahorrar memoria
            return await _context.Users.AsNoTracking().OrderBy(user => user.FirstName).ToListAsync();
        }

        public async Task<UserEntity?> GetByEmailAsync(string email) {
            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Email == email);
        }

        public async Task<UserEntity?> GetByIdAsync(int id) {
            return await _context.Users.FirstOrDefaultAsync(user => user.Id == id);
        }

        public async Task<int> SaveChangesAsync() {
            return await _context.SaveChangesAsync();
        }

        public Task UpdateAsync(UserEntity entity) {
            if (entity == null) {
                throw new ArgumentNullException(nameof(entity));
            }
            _context.Users.Update(entity);
            return Task.CompletedTask;
        }
    }
}
