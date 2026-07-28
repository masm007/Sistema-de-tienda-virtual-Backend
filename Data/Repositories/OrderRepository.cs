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
    public class OrderRepository : IOrderRepository<OrderEntity, int> {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext dbContext) {
            _context = dbContext;
        }

        public async Task CreateAsync(OrderEntity entity) {
            if (entity == null) throw new ArgumentNullException();
            await _context.Orders.AddAsync(entity);
        }

        public Task DeleteAsync(OrderEntity entity) {
            if (entity == null) {
                throw new ArgumentNullException(nameof(entity));
            }
            _context.Orders.Remove(entity);
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<OrderEntity>> GetAllAsync() {
            return await _context.Orders.AsNoTracking().Include(ord => ord.OrderDetails)
                .OrderBy(ord => ord.Id).ToListAsync();
        }

        public async Task<IEnumerable<OrderEntity>> GetAllByUserIdAsync(int userId) {
            return await _context.Orders.AsNoTracking().Include(ord => ord.OrderDetails)
                .Where(ord => ord.UserId == userId).OrderBy(ord => ord.Id).ToListAsync();
        }

        public async Task<OrderEntity?> GetByIdAsync(int id) {
            return await _context.Orders.Include(ord => ord.OrderDetails)
                .FirstOrDefaultAsync(ord => ord.Id == id);
        }

        public async Task<OrderEntity?> GetByOrderNumberAsync(string orderNumber) {
            return await _context.Orders.Include(ord => ord.OrderDetails)
                .FirstOrDefaultAsync(ord => ord.OrderNumber == orderNumber);
        }

        public async Task<int> SaveChangesAsync() {
            return await _context.SaveChangesAsync();
        }

        public Task UpdateAsync(OrderEntity entity) {
            if (entity == null) {
                throw new ArgumentNullException(nameof(entity));
            }
            _context.Orders.Update(entity);
            return Task.CompletedTask;
        }
    }
}
