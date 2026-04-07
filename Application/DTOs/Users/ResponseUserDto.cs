using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Users {
    public class ResponseUserDto {
        public string FirstName { get; private  set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public string AccessToken { get; private set; }
        public ResponseUserDto(string firstName, string lastName, string email, string jwtToken) {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            AccessToken = jwtToken;
        }
    }
}
