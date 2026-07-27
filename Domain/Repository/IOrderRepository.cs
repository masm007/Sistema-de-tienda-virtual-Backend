using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repository {
    public interface IOrderRepository<TEntity, TId> where TEntity : class {
        Task<TEntity?> GetByIdAsync(TId id);
        Task<TEntity?> GetByOrderNumberAsync(string orderNumber);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IEnumerable<TEntity>> GetAllByUserIdAsync(int userId);
        Task CreateAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
        Task<int> SaveChangesAsync();

    }
}
