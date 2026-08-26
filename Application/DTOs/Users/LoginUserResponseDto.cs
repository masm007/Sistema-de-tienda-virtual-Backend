using Domain.Entity;
using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Users {
    public class LoginUserResponseDto {
        //lo que necesita AuthResult
        public string FirstName { get; private  set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public UserRole Role { get; private set; }
        public string AccessToken { get; private set; }
        //no es seguro
        //public string RefreshToken { get; private set; }
        public LoginUserResponseDto(string firstName, string lastName, string email, 
            UserRole role, string jwtToken) {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Role = role;
            AccessToken = jwtToken;
        }
    }
}
