using Application.DTOs.Categories;
using Application.DTOs.Images;
using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Products {
    public class ProductDto {
        //esta clase es lo que recibirá el frontend
        //deberia ser lo que recibe el usuario admin
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Sku { get; private set; }
        public string Description { get; private set; }
        public CategorySummaryDto Category { get; private set; }
        public decimal Price { get; private set; }
        public int Quantity { get; private set; }
        public bool IsAvailable { get; private set; }
        public bool IsActive { get; private set; }
        //retorna unicamente la url de la imagen del producto
        public List<ProductImageDto> Images { get; private set; } = [];

        public ProductDto(int id, string name, string sku ,string description, CategorySummaryDto category, decimal price,
            int quantity, bool isAvailable, bool isActive, List<ProductImageDto> images) {
            Id = id;
            Name = name;
            Sku = sku;
            Description = description;
            Category = category;
            Price = price;
            Quantity = quantity;
            IsAvailable = isAvailable;
            IsActive = isActive;
            Images = images;
        }

        public ProductDto(int id, string name, string description, CategorySummaryDto category, decimal price, int quantity, 
            bool isAvailable, bool isActive) {
            Id = id;
            Name = name;
            Description = description;
            Category = category;
            Price = price;
            Quantity = quantity;
            IsAvailable = isAvailable;
            IsActive = isActive;
        }
    }
}
