using Application.DTOs.Users;
using Application.Interfaces;
using Domain.Entity;
using Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Users {
    public class LoginUserUseCase {
        private readonly IRepository<UserEntity, int> _repository;
        private readonly IJwtService _jwtService;
        private readonly IPasswordHasher _passwordHasher;

        public LoginUserUseCase(IRepository<UserEntity, int> repository, IJwtService jwtService,
                IPasswordHasher passwordHasher) {
            _repository = repository;
            _jwtService = jwtService;
            _passwordHasher = passwordHasher;
        }
        public async Task<ResponseLoginUserDto?> Execute(LoginUserDto loginUserDto) {
            var normalizedEmail = loginUserDto.Email.Trim().ToLowerInvariant();
            var user = await _repository.GetByEmailAsync(normalizedEmail);
            if (user == null) {
                throw new UnauthorizedAccessException("Credenciales inválidas");
            }
            if (!_passwordHasher.Verify(loginUserDto.Password, user.Password)) {
                throw new UnauthorizedAccessException("Credenciales inválidas");
            }
            /*
            if (user.Password != loginUserDto.Password) {
                throw new UnauthorizedAccessException("Credenciales inválidas");
            }
            */
            var token = _jwtService.GenerateToken(user);
            var usuario = new ResponseLoginUserDto(
                user.FirstName,
                user.LastName,
                user.Email,
                token
            );
            return usuario;
        }
    }
}
