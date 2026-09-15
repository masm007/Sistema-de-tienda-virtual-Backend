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
    public class GetAllCouponsUseCase {
        private readonly ICouponRepository<CouponEntity, int> _repository;
        public GetAllCouponsUseCase(ICouponRepository<CouponEntity, int> repository) {
            _repository = repository;
        }

        public async Task<IEnumerable<CouponDto>> ExecuteAsync() {
            var coupons = await _repository.GetAllAsync();
            var response = new List<CouponDto>();
            foreach (var c in coupons) {
                var products = c.RequiredProducts
                    .Select(rp => new ProductSummaryDto(rp.Product.Id, rp.Product.Name))
                    .ToList();
                response.Add(new CouponDto(c.Id, c.Code, c.Type, c.DiscountValue, c.ExpirationDate,
                    c.IsActive, c.UsageLimit, c.UsedCount, products));
            }
            return response;
        }
    }
}
