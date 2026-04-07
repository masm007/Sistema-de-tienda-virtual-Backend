using Domain.Entity;
using Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Users {
    public class GetUserByIdUseCase {
        private readonly IRepository<UserEntity, int> _repository;
        public GetUserByIdUseCase(IRepository<UserEntity, int> repository) {
            _repository = repository;
        }

        public async Task<UserEntity?> ExecuteAsync(int id) {
            return await _repository.GetByIdAsync(id);
        }
    }
}
