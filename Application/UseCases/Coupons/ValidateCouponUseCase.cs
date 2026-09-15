using Application.DTOs.Coupons;
using Application.DTOs.OrderDetail;
using Application.Helpers;
using Domain.Entity;
using Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Coupons {
    public class ValidateCouponUseCase {
        private readonly ICouponRepository<CouponEntity, int> _couponRepository;
        private readonly IProductRepository<ProductEntity, int> _productRepository;

        public ValidateCouponUseCase(ICouponRepository<CouponEntity, int> couponRepository,
            IProductRepository<ProductEntity, int> productRepository) {
            _couponRepository = couponRepository;
            _productRepository = productRepository;
        }

        public async Task<ApplyCouponResultDto> ExecuteAsync(string code, List<OrderDetailRequestDto> details) {
            if (string.IsNullOrWhiteSpace(code)) {
                throw new ArgumentException("Debe indicar un código de cupón");
            }
            if (details == null || details.Count == 0) {
                throw new ArgumentException("El carrito no puede estar vacío");
            }
            var coupon = await _couponRepository.GetByCodeAsync(code.Trim().ToUpperInvariant());
            if (coupon == null) {
                throw new InvalidOperationException("Cupón no encontrado");
            }
            if (!coupon.IsValid()) {
                throw new InvalidOperationException("El cupón no es válido, expiró o alcanzó su límite de usos");
            }

            var previewDetails = new List<OrderDetailEntity>();
            foreach (var item in details) {
                var product = await _productRepository.GetByIdAsync(item.ProductId);
                if (product == null) {
                    throw new InvalidOperationException("Uno de los productos del carrito no existe");
                }
                previewDetails.Add(new OrderDetailEntity(product.Id, product.Price, item.Quantity));
            }

            var eligibleSubtotal = CouponCalculator.CalculateEligibleSubtotal(coupon, previewDetails);
            var discount = coupon.CalculateDiscount(eligibleSubtotal);
            return new ApplyCouponResultDto(coupon.Code, discount, eligibleSubtotal);
        }
    }
}
