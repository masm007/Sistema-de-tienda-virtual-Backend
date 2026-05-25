using Domain.Enum;
using Domain.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Domain.Entity {
    public class UserEntity {
        public int Id { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public string Password { get; private set; }
        public UserRole Role { get; private set; }
        public ICollection<RefreshTokenEntity> RefreshTokens { get; private set; } = new List<RefreshTokenEntity>();
        public string Fullname => $"{FirstName} {LastName}";

        //importante para EF
        private UserEntity() { }

        public UserEntity(string firstName, string lastName, string email, string password) {
            var normalizedEmail = NormalizeEmail(email);
            FieldsValidator.ValidateText(firstName, "nombre", 3, 100);
            FieldsValidator.ValidateText(lastName, "apellido", 2, 100);
            FieldsValidator.ValidateEmail(normalizedEmail);

            this.FirstName = firstName.Trim();
            this.LastName = lastName.Trim();
            this.Email = normalizedEmail;
            this.Password = password;
            this.Role = UserRole.User;
        }

        public UserEntity(int id, string firstName, string lastName, string email, string password) {
            var normalizedEmail = NormalizeEmail(email);
            FieldsValidator.ValidateText(firstName, "nombre", 3, 100);
            FieldsValidator.ValidateText(lastName, "apellido", 2, 100);
            FieldsValidator.ValidateEmail(normalizedEmail);

            this.Id = id;
            this.FirstName = firstName.Trim();
            this.LastName = lastName.Trim();
            this.Email = normalizedEmail;
            this.Password = password;
            this.Role = UserRole.User;
        }

        public void UpdatePersonalInfo(string firstName, string lastName, string email) {
            var normalizedEmail = NormalizeEmail(email);
            FieldsValidator.ValidateText(firstName, "nombre", 3, 100);
            FieldsValidator.ValidateText(lastName, "apellido", 2, 100);
            FieldsValidator.ValidateEmail(normalizedEmail);
            FirstName = firstName.Trim();
            LastName = lastName.Trim();
            Email = normalizedEmail;
        }

        public void UpdatePassword(string password) {
            if (string.IsNullOrWhiteSpace(password)) {
                throw new ArgumentException("La contraseña no puede ser vacía");
            }
            Password = password;
        }
        private string NormalizeEmail(string email) {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email inválido", nameof(email));

            return email.Trim().ToLowerInvariant();
        }

        public void MakeAdmin() {
            this.Role = UserRole.Admin;
        }

        public void MakeUser() {
            this.Role = UserRole.User;
        }
    }
}