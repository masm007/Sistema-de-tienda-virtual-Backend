using Application.DTOs.User;
using Application.Interfaces;
using Domain.Entity;
using Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Application.UseCases.Users {
    public class CreateUserUseCase {
        private readonly IRepository<UserEntity, int> _repository;
        private readonly IPasswordHasher _passwordHasher;

        public CreateUserUseCase(IRepository<UserEntity, int> repository, IPasswordHasher passwordHasher) {
            _repository = repository;
            _passwordHasher = passwordHasher;
        }

        public async Task<UserEntity> ExecuteAsync(CreateUserDto dto) {
            if (dto == null) {
                throw new ArgumentNullException(nameof(dto));
            }
            ValidatePlainPassword(dto.Password);
            var hashedPassword = _passwordHasher.Hash(dto.Password);
            var user = new UserEntity(dto.FirstName, dto.LastName, dto.Email, hashedPassword);
            await _repository.CreateAsync(user);
            await _repository.SaveChangesAsync();
            return user;
        }

        public void ValidatePlainPassword(string password) {
            string regexPass = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*[&*])[A-Za-z\d&*]{12,20}$";
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException
                    ($"La contrasenia no puede estar vacia", nameof(password));
            if (!Regex.IsMatch(password, regexPass)) throw new ArgumentException
                    ("La contrasenia no cumple con el formato adecuado (mas de 12 caracteres" +
                    " entre ellos: una mayuscula, una minuscula y un caracter especial)");

        }
    }
}
