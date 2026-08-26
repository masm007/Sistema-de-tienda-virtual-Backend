using Application.DTOs.Users;
using Domain.Entity;
using Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Users {
    public class GetUserByIdUseCase {
        private readonly IUserRepository<UserEntity, int> _repository;
        public GetUserByIdUseCase(IUserRepository<UserEntity, int> repository) {
            _repository = repository;
        }

        public async Task<UserResponseDto?> ExecuteAsync(int id) {
            var user = await _repository.GetByIdAsync(id);
            if (user == null) {
                throw new InvalidOperationException("Usuario no encontrado");
            }
            var responseUser = new UserResponseDto(user.Id,user.FirstName, user.LastName, user.Email, user.Role);
            return responseUser;
        }
    }
}
