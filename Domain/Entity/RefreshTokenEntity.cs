using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity {
    public class RefreshTokenEntity {
        public int Id { get; private set; }
        public int UserId { get; private set; }
        public string TokenHash { get; private set; }
        public DateTime Expiration { get; private set; }
        public bool IsRevoked { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public UserEntity User { get; private set; }

        //para EF core
        private RefreshTokenEntity() { }

        public RefreshTokenEntity(int userId, string tokenHash, DateTime expiration) {
            ValidarUserId(userId);
            ValidarTokenHash(tokenHash);
            ValidarExpiration(expiration);
            UserId = userId;
            TokenHash = tokenHash;
            Expiration = expiration;
            CreatedAt = DateTime.UtcNow;
            IsRevoked = false;
        }
        private void ValidarUserId(int userId) {
            if (userId <= 0) {
                throw new ArgumentException("UserId inválido");
            }
        }
        private void ValidarTokenHash(string tokenHash) {
            if (string.IsNullOrWhiteSpace(tokenHash)) {
                throw new ArgumentException("TokenHash inválido");
            }
        }
        private void ValidarExpiration(DateTime expiration) {
            if (expiration <= DateTime.UtcNow) {
                throw new ArgumentException("Expiration debe ser futura");
            }
        }
        public bool IsActive() {
            return !IsRevoked && !IsExpired();
        }
        public bool IsExpired() {
            return DateTime.UtcNow >= Expiration;
        }
        public void Revoke() {
            if (IsRevoked) return;
            IsRevoked = true;
        }
    }
}
