using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity {
    public class CouponProductEntity {
        public int Id { get; private set; }
        public int CouponId { get; private set; }
        public CouponEntity Coupon { get; private set; }
        public int ProductId { get; private set; }
        public ProductEntity Product { get; private set; }

        private CouponProductEntity() { }

        public CouponProductEntity(int productId) {
            ProductId = productId;
        }
    }
}
