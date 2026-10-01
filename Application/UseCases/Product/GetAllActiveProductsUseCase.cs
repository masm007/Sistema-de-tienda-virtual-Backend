using Application.DTOs.Images;
using Application.DTOs.Products;
using Application.Helpers;
using Domain.Entity;
using Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Product {
    public class GetAllActiveProductsUseCase {
        private IProductRepository<ProductEntity, int> _repository;
        private readonly ITaxSettingRepository _taxSettingRepository;

        public GetAllActiveProductsUseCase(IProductRepository<ProductEntity, int> repository,
            ITaxSettingRepository taxSettingRepository) {
            _repository = repository;
            _taxSettingRepository = taxSettingRepository;
        }

        public async Task<IEnumerable<ProductUserResponseDto>> ExecuteAsync() {
            var products = await _repository.GetAllActiveAsync();
            if (products == null) {
                throw new InvalidOperationException("Producto no encontrado");
            }
            var taxSetting = await _taxSettingRepository.GetAsync();
            var ivaPercentage = taxSetting?.IvaPercentage ?? 15m;
            var response = new List<ProductUserResponseDto>();
            foreach (var prd in products) {
                var images = new List<ProductImageDto>();
                foreach (var img in prd.Images) {
                    images.Add(new ProductImageDto(img.ImageUrl));
                }
                // El cliente ve siempre el precio final, con el IVA ya incluido.
                var priceWithTax = PriceCalculator.CalculatePriceWithTax(prd.Price, ivaPercentage);
                response.Add(new ProductUserResponseDto(prd.Sku, prd.Name, prd.Description, prd.Category.Name,
                priceWithTax, prd.Quantity, prd.IsAvailable, prd.IsActive, images));
            }
            return response;
        }
    }
}
