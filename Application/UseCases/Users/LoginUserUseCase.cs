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
        public LoginUserUseCase(IRepository<UserEntity, int> repository, IJwtService jwtService) {
            _repository = repository;
            _jwtService = jwtService;
        }
        public async Task<ResponseUserDto?> Execute(LoginUserDto loginUserDto) {
            var user = await _repository.GetByEmailAsync(loginUserDto.Email);
            if (user == null) {
                throw new UnauthorizedAccessException("Credenciales inválidas");
            }
            if (user.Password != loginUserDto.Password) {
                throw new UnauthorizedAccessException("Credenciales inválidas");
            }
            var token = _jwtService.GenerateToken(user);
            var usuario = new ResponseUserDto(
                user.FirstName,
                user.LastName,
                user.Email,
                token
            );
            return usuario;
        }
    }
}
