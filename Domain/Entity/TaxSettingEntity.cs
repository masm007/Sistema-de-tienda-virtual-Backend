using System;

namespace Domain.Entity {
    public class TaxSettingEntity {
        public int Id { get; private set; }
        public decimal IvaPercentage { get; private set; }

        private TaxSettingEntity() { }

        public TaxSettingEntity(decimal ivaPercentage) {
            ValidatePercentage(ivaPercentage);
            IvaPercentage = ivaPercentage;
        }

        public void UpdatePercentage(decimal ivaPercentage) {
            ValidatePercentage(ivaPercentage);
            IvaPercentage = ivaPercentage;
        }

        private static void ValidatePercentage(decimal ivaPercentage) {
            if (ivaPercentage < 0 || ivaPercentage > 100) {
                throw new ArgumentException("El porcentaje de IVA debe estar entre 0 y 100");
            }
        }
    }
}
