using Application.Interfaces.Security;
using Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Users {
    public class LogoutUserUseCase {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IRefreshTokenHasher _refreshTokenHasher;
        public LogoutUserUseCase(IRefreshTokenRepository refreshTokenRepository, IRefreshTokenHasher refreshTokenHasher) {
            _refreshTokenRepository = refreshTokenRepository;
            _refreshTokenHasher = refreshTokenHasher;
        }

        public async Task Execute(string refreshToken) {
            if (string.IsNullOrWhiteSpace(refreshToken)) return;

            var hash = _refreshTokenHasher.Hash(refreshToken);
            var storedToken = await _refreshTokenRepository.GetByTokenHashAsync(hash);

            if (storedToken != null && !storedToken.IsRevoked) {
                storedToken.Revoke();
                await _refreshTokenRepository.SaveChangesAsync();
            }
        }
    }
}
