using Application.DTOs.Images;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Products {
    public class ProductUserResponseDto {
        //es lo que recibe el cliente en el frontend
        public string Sku { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string CategoryName { get; private set; }
        public decimal Price { get; private set; }
        public int Quantity { get; private set; }
        public bool IsAvailable { get; private set; }
        public bool IsActive { get; private set; }
        //retorna unicamente la url de la imagen del producto
        public List<ProductImageDto> Images { get; private set; } = [];

        public ProductUserResponseDto(string sku, string name, string description, string categoryName, 
            decimal price, int quantity, bool isAvailable, bool isActive, List<ProductImageDto> images) {
            Name = name;
            Sku = sku;
            Description = description;
            CategoryName = categoryName;
            Price = price;
            Quantity = quantity;
            IsAvailable = isAvailable;
            IsActive = isActive;
            Images = images;
        }
    }
}
