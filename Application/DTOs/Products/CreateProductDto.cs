using Application.DTOs.Images;
using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Products {
    public class CreateProductDto {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public int CategoryId { get; private set; }
        public decimal Price { get; private set; }
        public string Sku { get; private set; }
        public int Quantity { get; private set; }
        public List<ProductImageUploadDto> Images { get; private set; } = [];

        public CreateProductDto(string name, string description, decimal price,
        int quantity, List<ProductImageUploadDto> images, string sku, int categoryId) {
            Name = name;
            Description = description;
            CategoryId = categoryId;
            Price = price;
            Quantity = quantity;
            Images.AddRange(images);
            Sku = sku;
        }
    }
}
