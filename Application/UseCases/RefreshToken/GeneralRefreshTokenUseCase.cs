using Application.DTOs.Users;
using Application.Interfaces.Configuration;
using Application.Interfaces.Security;
using Domain.Entity;
using Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.RefreshToken {
    public class GeneralRefreshTokenUseCase {
        private readonly IRefreshTokenRepository _repository;
        private readonly IRefreshTokenHasher _hasher;
        private readonly IRefreshTokenSettings _settings;
        private readonly IJwtService _jwtService;
        private readonly IUserRepository<UserEntity, int> _userRepository;
        private readonly IRefreshTokenService _tokenService;

        public GeneralRefreshTokenUseCase(IRefreshTokenRepository repository, IRefreshTokenHasher hasher, 
            IRefreshTokenSettings settings, IJwtService jwtService, IUserRepository<UserEntity, int> userRepository,
            IRefreshTokenService tokenService) {
            _repository = repository;
            _hasher = hasher;
            _settings = settings;
            _jwtService = jwtService;
            _userRepository = userRepository;
            _tokenService = tokenService;
        }
        public async Task<AuthResult> Execute(string refreshToken) {
            if (string.IsNullOrWhiteSpace(refreshToken)) {
                throw new UnauthorizedAccessException();
            }
            var hash = _hasher.Hash(refreshToken);
            var storedToken = await _repository.GetByTokenHashAsync(hash);
            if (storedToken == null) {
                throw new UnauthorizedAccessException();
            }

            //verificar si el token no esta revocado (reuse attack)
            if (storedToken.IsRevoked) {
                //cerrar todas las sesiones del usuario
                await _tokenService.RevokeAllUserTokensAsync(storedToken.UserId);
                throw new UnauthorizedAccessException();
            }
            // expirado
            if (storedToken.IsExpired()) {
                throw new UnauthorizedAccessException();
            }
            //revocar el token
            storedToken.Revoke();
            await _repository.SaveChangesAsync();

            //obtener user
            var user = await _userRepository.GetByIdAsync(storedToken.UserId);
            if (user == null) {
                throw new UnauthorizedAccessException();
            }
            //nuevo token
            var newRefreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            var newHash = _hasher.Hash(newRefreshToken);
            var expiration = DateTime.UtcNow.AddDays(_settings.ExpirationDays);
            var newTokenEntity = new RefreshTokenEntity(
                user.Id,
                newHash,
                expiration
            );
            await _repository.CreateAsync(newTokenEntity);
            await _repository.SaveChangesAsync();
            //generar un nuevo JWT
            var jwt = _jwtService.GenerateToken(user);
            var responseLoginUser = new ResponseLoginUserDto(user.FirstName,user.LastName,user.Email,jwt);
            return new AuthResult(responseLoginUser, newRefreshToken);
        }
    }
}
