using Application.Interfaces.Security;
using Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Security {
    public class RefreshTokenService : IRefreshTokenService {
        private readonly IRefreshTokenRepository _repository;

        public RefreshTokenService(IRefreshTokenRepository repository) {
            _repository = repository;
        }

        public async Task RevokeAllUserTokensAsync(int userId) {
            var tokens = await _repository.GetByUserIdAsync(userId);
            foreach (var token in tokens) {
                token.Revoke();
            }
            await _repository.SaveChangesAsync();
        }
    }
}
