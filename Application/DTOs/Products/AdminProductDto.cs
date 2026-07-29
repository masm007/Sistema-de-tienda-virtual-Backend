using Application.DTOs.Images;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Products {
    public class AdminProductDto {
        //esta clase es lo que recibirá el frontend
        //recibe el usuario admin
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public int CategoryId { get; private set; }
        public decimal Price { get; private set; }
        public string Sku { get; private set; }
        public int Quantity { get; private set; }
        public bool IsAvailable { get; private set; }
        public bool IsActive { get; private set; }
        //retorna unicamente la url de la imagen del producto
        public List<ProductImageDto> Images { get; private set; } = [];

        public AdminProductDto(int id, string name, string description, int categoryId, decimal price, string sku, int quantity, bool isAvailable, bool isActive, List<ProductImageDto> images) {
            Id = id;
            Name = name;
            Description = description;
            CategoryId = categoryId;
            Price = price;
            Sku = sku;
            Quantity = quantity;
            IsAvailable = isAvailable;
            IsActive = isActive;
            Images = images;
        }

        public AdminProductDto(int id, string name, string description, int categoryId, decimal price, string sku, int quantity, bool isAvailable, bool isActive) {
            Id = id;
            Name = name;
            Description = description;
            CategoryId = categoryId;
            Price = price;
            Sku = sku;
            Quantity = quantity;
            IsAvailable = isAvailable;
            IsActive = isActive;
        }
    }
}
