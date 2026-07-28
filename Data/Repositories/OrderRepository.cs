using Data.Persistence;
using Domain.Entity;
using Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories {
    public class OrderRepository : IOrderRepository<OrderEntity, int> {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext dbContext) {
            _context = dbContext;
        }

        public async Task CreateAsync(OrderEntity entity) {
            if (entity == null) throw new ArgumentNullException();
            await _context.AddAsync(entity);
        }

        public Task DeleteAsync(OrderEntity entity) {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<OrderEntity>> GetAllAsync() {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<OrderEntity>> GetAllByUserIdAsync(int userId) {
            throw new NotImplementedException();
        }

        public Task<OrderEntity?> GetByIdAsync(int id) {
            throw new NotImplementedException();
        }

        public Task<OrderEntity?> GetByOrderNumberAsync(string orderNumber) {
            throw new NotImplementedException();
        }

        public Task<int> SaveChangesAsync() {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(OrderEntity entity) {
            throw new NotImplementedException();
        }
    }
}
