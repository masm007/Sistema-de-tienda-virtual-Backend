using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Security {
    public interface IRefreshTokenService {
        Task RevokeAllUserTokensAsync(int userId);

    }
}
