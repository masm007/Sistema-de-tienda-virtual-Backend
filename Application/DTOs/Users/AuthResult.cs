using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Users {
    public class AuthResult {
        //lo que se retorna al hacer un login exitoso
        public LoginUserResponseDto User { get; private set; }
        public string RefreshToken { get; private set; }

        public AuthResult(LoginUserResponseDto user, string refreshToken) {
            User = user;
            RefreshToken = refreshToken;
        }

    }
}
