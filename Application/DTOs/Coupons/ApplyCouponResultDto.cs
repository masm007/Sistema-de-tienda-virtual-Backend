using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Coupons {
    public class ApplyCouponResultDto {
        public string Code { get; private set; }
        public decimal EligibleSubtotal { get; private set; }
        public decimal DiscountAmount { get; private set; }

        public ApplyCouponResultDto(string code, decimal discountAmount, decimal eligibleSubtotal) {
            Code = code;
            DiscountAmount = discountAmount;
            EligibleSubtotal = eligibleSubtotal;
        }
    }
}
