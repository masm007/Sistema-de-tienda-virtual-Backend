using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Users {
    public class UserDto {
        //clase usada por la orden al devolver datos
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }

        public UserDto(string firstName, string lastName, string email) {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }
    }
}
