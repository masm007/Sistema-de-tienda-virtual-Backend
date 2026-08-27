using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repository {
    public interface IUserRepository<TEntity, TId> where TEntity : class {
        Task<TEntity?> GetByIdAsync(TId id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task CreateAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
        Task<TEntity?> GetByEmailAsync(string email);
        Task<int> SaveChangesAsync();
    }
}
