using Application.DTOs.Settings;
using Domain.Repository;
using System;
using System.Threading.Tasks;

namespace Application.UseCases.Settings {
    public class UpdateTaxSettingUseCase {
        private readonly ITaxSettingRepository _repository;

        public UpdateTaxSettingUseCase(ITaxSettingRepository repository) {
            _repository = repository;
        }

        public async Task<TaxSettingDto> ExecuteAsync(decimal ivaPercentage) {
            var setting = await _repository.GetAsync();
            if (setting == null) {
                throw new InvalidOperationException("No hay una configuración de impuestos definida");
            }
            setting.UpdatePercentage(ivaPercentage);
            await _repository.UpdateAsync(setting);
            await _repository.SaveChangesAsync();
            return new TaxSettingDto(setting.IvaPercentage);
        }
    }
}
