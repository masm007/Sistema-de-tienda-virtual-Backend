using Application.Interfaces.Configuration;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Configurations {
    public class RefreshTokenSettings : IRefreshTokenSettings {
        private readonly IConfiguration _config;

        public RefreshTokenSettings(IConfiguration config) {
            _config = config;
        }

        public int ExpirationDays =>
            int.Parse(_config["RefreshToken:Days"]);
    }
}
