using Domain.Entity;
using Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Coupons {
    public class DeactivateCouponUseCase {
        private readonly ICouponRepository<CouponEntity, int> _repository;
        public DeactivateCouponUseCase(ICouponRepository<CouponEntity, int> repository) {
            _repository = repository;
        }

        public async Task ExecuteAsync(int id) {
            var coupon = await _repository.GetByIdAsync(id);
            if (coupon == null) {
                throw new InvalidOperationException("Cupón no encontrado");
            }
            coupon.Deactivate();
            await _repository.UpdateAsync(coupon);
            await _repository.SaveChangesAsync();
        }
    }
}
