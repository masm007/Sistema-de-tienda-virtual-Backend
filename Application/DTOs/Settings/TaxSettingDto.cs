namespace Application.DTOs.Settings {
    public class TaxSettingDto {
        public decimal IvaPercentage { get; private set; }

        public TaxSettingDto(decimal ivaPercentage) {
            IvaPercentage = ivaPercentage;
        }
    }
}
