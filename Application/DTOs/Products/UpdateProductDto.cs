using Application.DTOs.Images;
using System.Collections.Generic;

namespace Application.DTOs.Products {
    public class UpdateProductDto {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public int CategoryId { get; private set; }
        public decimal Price { get; private set; }
        public string Sku { get; private set; }
        public int Quantity { get; private set; }
        public bool IsAvailable { get; private set; }
        public bool IsActive { get; private set; }
        // Ids de las imágenes existentes que se conservan; las que no estén aquí se eliminan
        public List<int> KeepImageIds { get; private set; } = [];
        // Archivos nuevos a subir
        public List<ProductImageUploadDto> NewImages { get; private set; } = [];

        public UpdateProductDto(int id, string name, string description, decimal price, int quantity,
        List<int> keepImageIds, List<ProductImageUploadDto> newImages, string sku, int categoryId,
        bool isAvailable, bool isActive) {
            Id = id;
            Name = name;
            Description = description;
            CategoryId = categoryId;
            Price = price;
            Quantity = quantity;
            KeepImageIds.AddRange(keepImageIds);
            NewImages.AddRange(newImages);
            Sku = sku;
            IsAvailable = isAvailable;
            IsActive = isActive;
        }
    }
}
