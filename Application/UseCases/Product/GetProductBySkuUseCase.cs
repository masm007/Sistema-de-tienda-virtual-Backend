using Application.DTOs.Categories;
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
    public class GetProductBySkuUseCase {
        private IProductRepository<ProductEntity, int> _repository;
        private readonly IProductImageRepository<ProductImageEntity, int> _imageRepository;
        private readonly ITaxSettingRepository _taxSettingRepository;

        public GetProductBySkuUseCase(IProductRepository<ProductEntity, int> repository,
            IProductImageRepository<ProductImageEntity, int> imageRepository,
            ITaxSettingRepository taxSettingRepository) {
            _repository = repository;
            _imageRepository = imageRepository;
            _taxSettingRepository = taxSettingRepository;
        }

        public async Task<ProductUserResponseDto?> ExecuteAsync(string sku) {
            var prd = await _repository.GetBySkuAsync(sku);
            if (prd == null) {
                throw new InvalidOperationException("Producto no encontrado");
            }
            var images = await _imageRepository.GetAllByProductIdAsync(prd.Id);
            //he estado usando mal Images pq no existe en bd
            var urlImages = new List<ProductImageDto>();
            foreach (var item in images) {
                urlImages.Add(new ProductImageDto(item.ImageUrl));
            }
            var taxSetting = await _taxSettingRepository.GetAsync();
            var ivaPercentage = taxSetting?.IvaPercentage ?? 15m;
            // El cliente ve siempre el precio final, con el IVA ya incluido.
            var priceWithTax = PriceCalculator.CalculatePriceWithTax(prd.Price, ivaPercentage);
            var response = new ProductUserResponseDto(prd.Sku, prd.Name, prd.Description, prd.Category.Name,
                priceWithTax, prd.Quantity, prd.IsAvailable, prd.IsActive, urlImages);
            return response;
        }
    }
}
