using Domain.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Domain.Entity {
    public class ProductEntity {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public int CategoryId { get; private set; }
        public decimal Price { get; private set; }
        public string Sku { get; private set; }
        public int Quantity { get; private set; }
        //aparezca en tienda pese a no tener stock
        public bool IsAvailable { get; private set; }
        //eliminacion logica
        public bool IsActive { get; private set; }
        public List<ProductImageEntity> Images { get; private set; } = [];

        public CategoryEntity Category { get; private set; }

        private ProductEntity() { }

        public ProductEntity(string name, string description, decimal price,
        int quantity, List<ProductImageEntity> images, string sku, int categoryId) {
            string normalizedSku = ValidateProduct(name, description, price, quantity, sku, images);
            Name = name;
            Description = description;
            CategoryId = categoryId;
            Price = price;
            Quantity = quantity;
            IsAvailable = true;
            IsActive = true;
            Images.AddRange(images);
            Sku = normalizedSku;
        }

        public void UpdateInfo(string name, string description, decimal price, int categoryId,
        int quantity, bool isAvailable, bool isActive, List<ProductImageEntity> images, string sku) {
            string normalizedSku = ValidateProduct(name, description, price, quantity, sku, images);
            Name = name;
            Description = description;
            CategoryId = categoryId;
            Price = price;
            Quantity = quantity;
            IsAvailable = isAvailable;
            IsActive = isActive;
            Sku = normalizedSku;
            Images.Clear();
            Images.AddRange(images);
        }

        private static string ValidateProduct(string name, string description, decimal price,
            int quantity, string sku, ICollection<ProductImageEntity> images) {
            FieldsValidator.ValidateText(name, "nombre", 5, 50);
            FieldsValidator.ValidateText(description, "descripcion", 20, 100);
            FieldsValidator.ValidateNumber(price,"precio",0,50);
            FieldsValidator.ValidateNumber(quantity,"cantidad",1);
            string newSku = ValidateSku(sku);
            ValidateImages(images);
            return newSku;
        }

        private static string ValidateSku(string sku) {
            if (string.IsNullOrWhiteSpace(sku))
                throw new ArgumentException("El SKU no puede estar vacío");

            sku = sku.Trim().ToUpper();

            if (sku.Length > 40)
                throw new ArgumentException("El SKU no puede superar los 40 caracteres");

            const string pattern = @"^[A-Z]{3,10}-[A-Z0-9]{3,20}-\d{3,5}$";

            if (!Regex.IsMatch(sku, pattern))
                throw new ArgumentException("El SKU no tiene un formato válido. Debe seguir: CAT-PRODUCTO-NUM (Ej: ELEC-MOUSE-001)");
            
            return sku;
        }

        private static void ValidateImages(ICollection<ProductImageEntity> images) {
            FieldsValidator.ValidateCollection(images, "Imágenes", 1, 5);
            if (images.Any(x => x is null)) {
                throw new ArgumentException("Existen imágenes inválidas");
            }
            if (images.GroupBy(x => x.CloudinaryPublicId).Any(x => x.Count() > 1)) {
                throw new ArgumentException("No se permiten imágenes duplicadas");
            }
        }
    }
}
