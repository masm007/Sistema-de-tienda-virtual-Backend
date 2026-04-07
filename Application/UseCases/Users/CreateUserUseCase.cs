using Application.DTOs.User;
using Domain.Entity;
using Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Users {
    public class CreateUserUseCase {
        private readonly IRepository<UserEntity, int> _repository;

        public CreateUserUseCase(IRepository<UserEntity, int> repository) {
            _repository = repository;
        }

        public async Task<UserEntity> ExecuteAsync(CreateUserDto dto) {
            var user = new UserEntity(dto.FirstName, dto.LastName, dto.Email, dto.Password);
            await _repository.CreateAsync(user);
            await _repository.SaveChangesAsync();
            return user;
        }
    }
}
