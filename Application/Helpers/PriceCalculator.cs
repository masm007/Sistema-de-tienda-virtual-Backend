using System;

namespace Application.Helpers {
    public static class PriceCalculator {
        // Precio final que ve el cliente: el precio base más el IVA vigente.
        public static decimal CalculatePriceWithTax(decimal basePrice, decimal ivaPercentage) {
            return Math.Round(basePrice * (1 + ivaPercentage / 100m), 2);
        }
    }
}
