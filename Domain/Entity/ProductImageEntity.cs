using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity {
    public class ProductImageEntity {
        public int Id { get; private set; }
        public string CloudinaryPublicId { get; private set;}
        public string ImageUrl { get; private set; }
        public int ProductId { get; private set;}
        public ProductEntity Product { get; private set;}

        private ProductImageEntity() { }

        public ProductImageEntity(int id, string cloudinaryPublicId, string imageUrl, 
            int productId) {
            Id = id;
            CloudinaryPublicId = cloudinaryPublicId;
            ImageUrl = imageUrl;
            ProductId = productId;
        }

        public ProductImageEntity(string cloudinaryPublicId, string imageUrl) {
            CloudinaryPublicId = cloudinaryPublicId;
            ImageUrl = imageUrl;
        }
    }
}
