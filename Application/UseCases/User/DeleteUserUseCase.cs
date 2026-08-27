using Application.DTOs.User;
using Domain.Entity;
using Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Users {
    public class DeleteUserUseCase {
        private readonly IUserRepository<UserEntity, int> _repository;

        public DeleteUserUseCase(IUserRepository<UserEntity, int> repository) {
            _repository = repository;
        }

        public async Task ExecuteAsync(int id) {
            var user = await _repository.GetByIdAsync(id);
            if (user == null) {
                throw new ArgumentNullException("No se encontro a una persona con ese Id");
            }
            await _repository.DeleteAsync(user);
            await _repository.SaveChangesAsync();
        }
    }
}
