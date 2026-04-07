using System;
using System.Collections.Generic;
using System.Linq;
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
        public string Fullname => $"{FirstName} {LastName}";
        public UserEntity() { }

        public UserEntity(string firstName, string lastName, string email, string pass) {
            this.FirstName = firstName.Trim();
            this.LastName = lastName.Trim();
            this.Email = email.Trim();
            this.Password = pass.Trim();
        }
        public UserEntity(int id, string firstName, string lastName, string email, string pass) {
            ValidateName(firstName, "nombre");
            ValidateName(lastName, "apellido");
            ValidateEmail(email);
            this.Id = id;
            this.FirstName = firstName.Trim();
            this.LastName = lastName.Trim();
            this.Email = email.Trim();
            this.Password = pass.Trim();
        }

        public void ValidateName(string name, string property) {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException($"El {property} no puede estar vacio", nameof(name));
            if (name.Trim().Length < 3) throw new ArgumentException($"El {property} no puede tener menos de 3 caracteres");
            if (name.Trim().Length >= 50) throw new ArgumentException($"El {property} no puede superar los 19 caracteres");
        }

        public void ValidateEmail(string email) {
            string regexEmail = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException($"El correo no puede estar vacio", nameof(email));
            if (email.Trim().Length < 15 || email.Trim().Length >= 100) throw new ArgumentException("La longitud de ese correo no es valida tiene que ser mayor a 15 y menor a 100");
            if (!Regex.IsMatch(email, regexEmail)) throw new ArgumentException("El correo no cumple con el formato adecuado");
        }

        public void ValidatePassword(string password) {
            string regexPass = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*[&*])[A-Za-z\d&*]{12,20}$";
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException($"La contrasenia no puede estar vacia", nameof(password));
            if (!Regex.IsMatch(password, regexPass)) throw new ArgumentException("La contrasenia no cumple con el formato adecuado (una mayuscula, una minuscula o un caracter especial)");

        }

        public void UpdatePersonalInfo(string firstName, string lastName, string email, string pass) {
            ValidateName(firstName, "nombre");
            ValidateName(lastName, "apellido");
            ValidateEmail(email);
            FirstName = firstName.Trim();
            LastName = lastName.Trim();
            Email = email.Trim();
            Password = pass.Trim();
        }
    }
}