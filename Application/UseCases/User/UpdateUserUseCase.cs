using Application.DTOs.User;
using Application.Interfaces.Security;
using Domain.Entity;
using Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Application.UseCases.Users {
    public class UpdateUserUseCase {
        private readonly IRepository<UserEntity, int> _repository;
        private readonly IPasswordHasher _passwordHasher;

        public UpdateUserUseCase(IRepository<UserEntity, int> repository, 
            IPasswordHasher passwordHasher) {
            _repository = repository;
            _passwordHasher = passwordHasher;
        }

        public async Task<UserEntity> ExecuteAsync(EditUserDto dto) {
            var user = await _repository.GetByIdAsync(dto.Id);
            if (user == null) {
                throw new ArgumentException("No se encontro a una persona con ese Id");
            }
            user.UpdatePersonalInfo(dto.FirstName, dto.LastName, dto.Email);

            string? hashedPassword = null;

            if (!string.IsNullOrWhiteSpace(dto.Password)) {
                ValidatePlainPassword(dto.Password);
                hashedPassword = _passwordHasher.Hash(dto.Password);
                user.UpdatePassword(hashedPassword);
            }
            await _repository.UpdateAsync(user);
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
