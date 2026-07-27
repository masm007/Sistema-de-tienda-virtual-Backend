using Application.DTOs.Users;
using Domain.Entity;
using Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Users {
    public class GetAllUsersUseCase {
        private readonly IUserRepository<UserEntity, int> _repository;
        public GetAllUsersUseCase(IUserRepository<UserEntity, int> repository) {
            _repository = repository;
        }

        public async Task<IEnumerable<ResponseUserDto>> ExecuteAsync() {
            var users = await _repository.GetAllAsync();
            List<ResponseUserDto> responseUsers = new List<ResponseUserDto>();
            foreach (var user in users) {
                var responseUser = new ResponseUserDto(user.Id, user.FirstName, user.LastName, user.Email, user.Role);
                responseUsers.Add(responseUser);
            }
            return responseUsers;
        }
    }
}
