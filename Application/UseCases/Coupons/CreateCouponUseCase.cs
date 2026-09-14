using Application.DTOs.Coupons;
using Application.DTOs.Products;
using Domain.Entity;
using Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Coupons {
    public class CreateCouponUseCase {
        private readonly ICouponRepository<CouponEntity, int> _couponRepository;
        private readonly IProductRepository<ProductEntity, int> _productRepository;

        public CreateCouponUseCase(ICouponRepository<CouponEntity, int> couponRepository,
            IProductRepository<ProductEntity, int> productRepository) {
            _couponRepository = couponRepository;
            _productRepository = productRepository;
        }

        public async Task<CouponDto> ExecuteAsync(CreateCouponDto dto) {
            if (dto == null) {
                throw new ArgumentNullException(nameof(dto));
            }
            var normalizedCode = dto.Code?.Trim().ToUpperInvariant() ?? string.Empty;
            var existing = await _couponRepository.GetByCodeAsync(normalizedCode);
            if (existing != null) {
                throw new InvalidOperationException("Ya existe un cupón con ese código");
            }

            var products = new List<ProductSummaryDto>();
            foreach (var productId in dto.ProductIds.Distinct()) {
                var product = await _productRepository.GetByIdAsync(productId);
                if (product == null) {
                    throw new InvalidOperationException($"El producto con id {productId} no existe");
                }
                products.Add(new ProductSummaryDto(product.Id, product.Name));
            }

            var coupon = new CouponEntity(normalizedCode, dto.Type, dto.DiscountValue,
                dto.ExpirationDate, dto.ProductIds, dto.UsageLimit);
            await _couponRepository.CreateAsync(coupon);
            await _couponRepository.SaveChangesAsync();

            return new CouponDto(coupon.Id, coupon.Code, coupon.Type, coupon.DiscountValue,
                coupon.ExpirationDate, coupon.IsActive, coupon.UsageLimit, coupon.UsedCount, products);
            }
        }
    }
