using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repository {
    public interface IProductRepository<TEntity, TId> where TEntity : class {
        Task<TEntity?> GetByIdAsync(TId id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IEnumerable<TEntity>> GetAllActiveAsync();
        Task<IEnumerable<TEntity>> GetAllByCategoryIdAsync(TId categoryId);
        Task CreateAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
        Task<int> SaveChangesAsync();
    }
}
