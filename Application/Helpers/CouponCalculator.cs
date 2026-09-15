using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helpers {
    public class CouponCalculator {
        public static decimal CalculateEligibleSubtotal(CouponEntity coupon, IEnumerable<OrderDetailEntity> orderDetails) {
            var requiredIds = coupon.RequiredProducts.Select(rp => rp.ProductId).ToHashSet();
            var cartIds = orderDetails.Select(d => d.ProductId).ToHashSet();

            if (!requiredIds.All(id => cartIds.Contains(id))) {
                throw new InvalidOperationException(
                    "El carrito debe incluir todos los productos requeridos por el cupón");
            }
            return orderDetails.Where(d => requiredIds.Contains(d.ProductId)).Sum(d => d.Subtotal);
        }
    }
}
