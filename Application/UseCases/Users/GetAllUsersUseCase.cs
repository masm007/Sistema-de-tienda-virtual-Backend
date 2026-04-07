using Domain.Entity;
using Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Users {
    public class GetAllUsersUseCase {
        private readonly IRepository<UserEntity, int> _repository;
        public GetAllUsersUseCase(IRepository<UserEntity, int> repository) {
            _repository = repository;
        }

        public async Task<IEnumerable<UserEntity>> ExecuteAsync() {
            return await _repository.GetAllAsync();
        }
    }
}
