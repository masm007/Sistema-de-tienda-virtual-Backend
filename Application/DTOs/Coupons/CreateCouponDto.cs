using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Coupons {
    public class CreateCouponDto {
        public string Code { get; set; }
        public DiscountType Type { get; set; }
        public decimal DiscountValue { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int? UsageLimit { get; set; }
        public List<int> ProductIds { get; set; } = [];
    }
}
