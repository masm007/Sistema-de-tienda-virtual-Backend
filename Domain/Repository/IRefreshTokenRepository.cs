using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repository {
    public interface IRefreshTokenRepository {
        Task CreateAsync(RefreshTokenEntity token);
        Task<RefreshTokenEntity?> GetByTokenHashAsync(string tokenHash);
        Task<IEnumerable<RefreshTokenEntity>> GetByUserIdAsync(int userId);
        Task<int> SaveChangesAsync();
    }
}
