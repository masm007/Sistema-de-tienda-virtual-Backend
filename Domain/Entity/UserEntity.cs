using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Net.Mail;
using Domain.Enum;

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
            ValidateName(firstName, "nombre");
            ValidateName(lastName, "apellido");
            ValidateEmail(normalizedEmail);

            this.FirstName = firstName.Trim();
            this.LastName = lastName.Trim();
            this.Email = normalizedEmail;
            this.Password = password;
            this.Role = UserRole.User;
        }

        public UserEntity(int id, string firstName, string lastName, string email, string password) {
            var normalizedEmail = NormalizeEmail(email);
            ValidateName(firstName, "nombre");
            ValidateName(lastName, "apellido");
            ValidateEmail(normalizedEmail);

            this.Id = id;
            this.FirstName = firstName.Trim();
            this.LastName = lastName.Trim();
            this.Email = normalizedEmail;
            this.Password = password;
            this.Role = UserRole.User;
        }

        public void ValidateName(string name, string property) {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException($"El {property} no puede estar vacio", nameof(name));
            if (name.Trim().Length < 3) throw new ArgumentException($"El {property} no puede tener menos de 3 caracteres");
            if (name.Trim().Length >= 100) throw new ArgumentException($"El {property} no puede superar los 100 caracteres");
        }

        public void ValidateEmail(string email) {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("El correo no puede estar vacío", nameof(email));

            if (email.Length > 100)
                throw new ArgumentException("El correo no puede superar los 100 caracteres");

            try {
                var addr = new MailAddress(email);
            } catch {
                throw new ArgumentException("El correo no tiene un formato válido");
            }
        }

        /*
        public void UpdatePersonalInfo(string firstName, string lastName, string email, string pass) {
            var normalizedEmail = email.Trim().ToLowerInvariant();
            ValidateName(firstName, "nombre");
            ValidateName(lastName, "apellido");
            ValidateEmail(normalizedEmail);
            FirstName = firstName.Trim();
            LastName = lastName.Trim();
            Email = normalizedEmail;
            Password = pass;
        }
        */

        public void UpdatePersonalInfo(string firstName, string lastName, string email) {
            var normalizedEmail = NormalizeEmail(email);
            ValidateName(firstName, "nombre");
            ValidateName(lastName, "apellido");
            ValidateEmail(normalizedEmail);
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