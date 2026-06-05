using Application.DTOs.Users;
using Domain.Entity;
using Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;
using Application.Interfaces.Security;
using Application.Interfaces.Configuration;

namespace Application.UseCases.Users {
    public class LoginUserUseCase {
        private readonly IRepository<UserEntity, int> _repository;
        private readonly IRefreshTokenSettings _rtSettings;
        private readonly IJwtService _jwtService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IRefreshTokenHasher _refreshTokenHasher;
        private readonly IRefreshTokenService _refreshTokenService;

        public LoginUserUseCase(IRepository<UserEntity, int> repository, IJwtService jwtService,
                IPasswordHasher passwordHasher, IRefreshTokenRepository refreshTokenRepository,
                IRefreshTokenSettings rtSettings, IRefreshTokenHasher refreshTokenHasher, 
                IRefreshTokenService service) {
            _repository = repository;
            _jwtService = jwtService;
            _passwordHasher = passwordHasher;
            _refreshTokenRepository = refreshTokenRepository;
            _rtSettings = rtSettings;
            _refreshTokenHasher = refreshTokenHasher;
            _refreshTokenService = service;
        }

        public async Task<AuthResult> Execute(LoginUserDto loginUserDto) {
            if (loginUserDto == null) {
                throw new ArgumentNullException(nameof(loginUserDto));
            }
            var normalizedEmail = loginUserDto.Email.Trim().ToLowerInvariant();
            var user = await _repository.GetByEmailAsync(normalizedEmail);
            if (user == null) {
                throw new UnauthorizedAccessException("Credenciales inválidas");
            }
            if (!_passwordHasher.Verify(loginUserDto.Password, user.Password)) {
                throw new UnauthorizedAccessException("Credenciales inválidas");
            }
            await _refreshTokenService.RevokeAllUserTokensAsync(user.Id);
            //creacion del refresh token
            var randomBytes = RandomNumberGenerator.GetBytes(64);
            var refreshToken = Convert.ToBase64String(randomBytes);
            var hashToken = _refreshTokenHasher.Hash(refreshToken);
            var days = _rtSettings.ExpirationDays;
            var expirationDate = DateTime.UtcNow.AddDays(days);
            var refreshTokenEntity = new RefreshTokenEntity(user.Id, hashToken, expirationDate);
            await _refreshTokenRepository.CreateAsync(refreshTokenEntity);
            await _refreshTokenRepository.SaveChangesAsync();
            //creacion del token
            var token = _jwtService.GenerateToken(user);
            //objeto a devolver
            var responseLoginUser = new ResponseLoginUserDto(user.FirstName, user.LastName, user.Email, token);
            return new AuthResult(responseLoginUser, refreshToken);
        }
    }
}
