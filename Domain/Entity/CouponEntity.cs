using Domain.Enum;
using Domain.Validations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Entity {
    public class CouponEntity {
        public int Id { get; private set; }
        public string Code { get; private set; }
        public DiscountType Type { get; private set; }
        public decimal DiscountValue { get; private set; }
        public DateTime ExpirationDate { get; private set; }
        public bool IsActive { get; private set; }
        public int? UsageLimit { get; private set; }
        public int UsedCount { get; private set; }
        public ICollection<CouponProductEntity> RequiredProducts { get; private set; } = new List<CouponProductEntity>();

        private CouponEntity() { }

        public CouponEntity(string code, DiscountType type, decimal discountValue,
            DateTime expirationDate, List<int> requiredProductIds, int? usageLimit = null) {
            ValidateCode(code);
            ValidateDiscountValue(type, discountValue);
            ValidateExpiration(expirationDate);
            ValidateRequiredProducts(requiredProductIds);
            if (usageLimit is <= 0) {
                throw new ArgumentException("El límite de usos debe ser mayor a cero");
            }

            Code = code.Trim().ToUpperInvariant();
            Type = type;
            DiscountValue = discountValue;
            ExpirationDate = expirationDate;
            IsActive = true;
            UsageLimit = usageLimit;
            UsedCount = 0;

            foreach (var productId in requiredProductIds.Distinct()) {
                RequiredProducts.Add(new CouponProductEntity(productId));
            }
        }

        private static void ValidateCode(string code) {
            FieldsValidator.ValidateText(code, "código", 3, 30);
        }

        private static void ValidateDiscountValue(DiscountType type, decimal discountValue) {
            if (type == DiscountType.Percentage) {
                FieldsValidator.ValidateNumber(discountValue, "porcentaje de descuento", 1, 100);
            } else {
                FieldsValidator.ValidateNumber(discountValue, "monto de descuento", 0.01m);
            }
        }

        private static void ValidateExpiration(DateTime expirationDate) {
            if (expirationDate <= DateTime.UtcNow) {
                throw new ArgumentException("La fecha de expiración debe ser futura");
            }
        }

        private static void ValidateRequiredProducts(List<int> productIds) {
            if (productIds == null || productIds.Count == 0) {
                throw new ArgumentException("El cupón debe estar asociado al menos a un producto");
            }
            if (productIds.Distinct().Count() != productIds.Count) {
                throw new ArgumentException("No se pueden repetir productos en el mismo cupón");
            }
        }

        public bool IsExpired() => DateTime.UtcNow >= ExpirationDate;

        public bool IsValid() {
            if (!IsActive || IsExpired()) return false;
            if (UsageLimit.HasValue && UsedCount >= UsageLimit.Value) return false;
            return true;
        }

        // Calcula el descuento solo sobre el subtotal de los productos que exige el cupón
        public decimal CalculateDiscount(decimal eligibleSubtotal) {
            if (eligibleSubtotal <= 0) return 0;
            decimal discount = Type == DiscountType.Percentage
                ? eligibleSubtotal * (DiscountValue / 100m)
                : DiscountValue;
            // Nunca debe superar el subtotal de esos productos
            return Math.Min(discount, eligibleSubtotal);
        }

        public void RegisterUsage() {
            if (!IsValid()) {
                throw new InvalidOperationException("No se puede usar un cupón inválido, expirado o agotado");
            }
            UsedCount++;
        }

        public void Deactivate() => IsActive = false;
        public void Activate() => IsActive = true;
    }
}