using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Users {
    public class UserResponseDto {
        //solo usado para el admin
        public int Id { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public UserRole Role { get; private set; }
        public string Fullname => $"{FirstName} {LastName}";

        public UserResponseDto(int id, string firstName, string lastName, string email, UserRole role) {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Role = role;
        }
    }
}
