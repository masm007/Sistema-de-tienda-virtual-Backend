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
    public class RefreshTokenRepository : IRefreshTokenRepository {
        private readonly ApplicationDbContext _context;

        public RefreshTokenRepository(ApplicationDbContext dbContext) {
            _context = dbContext;
        }

        public async Task CreateAsync(RefreshTokenEntity token) {
            if (token == null) {
                throw new ArgumentNullException(nameof(token));
            }
            await _context.RefreshTokens.AddAsync(token);
        }
        //sin AsNoTracking() para que rastreen de parte de ef core posibles cambios
        public async Task<RefreshTokenEntity?> GetByTokenHashAsync(string tokenHash) {
            return await _context.RefreshTokens.FirstOrDefaultAsync(rToken => rToken.TokenHash == tokenHash);
        }

        public async Task<IEnumerable<RefreshTokenEntity>> GetByUserIdAsync(int userId) {
            return await _context.RefreshTokens.Where(rToken => rToken.UserId == userId).ToListAsync();
        }

        public async Task<int> SaveChangesAsync() {
            return await _context.SaveChangesAsync();
        }
    }
}
