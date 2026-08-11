using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repository {
    public interface IOrderRepository<TEntity, TId> where TEntity : class {
        Task<TEntity?> GetByOrderNumberForAdminAsync(TId id);
        Task<TEntity?> GetByOrderNumberForUserAsync(TId id, int userId);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IEnumerable<TEntity>> GetAllByUserIdAsync(int userId);
        Task CreateAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
        Task CreateWithNextNumberAsync(OrderEntity entity);
        Task<int> SaveChangesAsync();

    }
}
