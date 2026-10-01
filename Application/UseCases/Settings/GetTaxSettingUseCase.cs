using Application.DTOs.Settings;
using Domain.Repository;
using System;
using System.Threading.Tasks;

namespace Application.UseCases.Settings {
    public class GetTaxSettingUseCase {
        private readonly ITaxSettingRepository _repository;

        public GetTaxSettingUseCase(ITaxSettingRepository repository) {
            _repository = repository;
        }

        public async Task<TaxSettingDto> ExecuteAsync() {
            var setting = await _repository.GetAsync();
            if (setting == null) {
                throw new InvalidOperationException("No hay una configuración de impuestos definida");
            }
            return new TaxSettingDto(setting.IvaPercentage);
        }
    }
}
