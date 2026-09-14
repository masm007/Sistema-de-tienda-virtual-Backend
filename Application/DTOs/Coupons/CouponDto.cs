using Application.DTOs.Products;
using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Coupons {
    public class CouponDto {
        public int Id { get; private set; }
        public string Code { get; private set; }
        public DiscountType Type { get; private set; }
        public decimal DiscountValue { get; private set; }
        public DateTime ExpirationDate { get; private set; }
        public bool IsActive { get; private set; }
        public int? UsageLimit { get; private set; }
        public int UsedCount { get; private set; }
        public List<ProductSummaryDto> RequiredProducts { get; private set; } = [];

        public CouponDto(int id, string code, DiscountType type, decimal discountValue,
            DateTime expirationDate, bool isActive, int? usageLimit, int usedCount,
            List<ProductSummaryDto> requiredProducts) {
            Id = id;
            Code = code;
            Type = type;
            DiscountValue = discountValue;
            ExpirationDate = expirationDate;
            IsActive = isActive;
            UsageLimit = usageLimit;
            UsedCount = usedCount;
            RequiredProducts = requiredProducts;
        }
    }
}
