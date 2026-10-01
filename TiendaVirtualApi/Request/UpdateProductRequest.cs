using System.Collections.Generic;

namespace TiendaVirtualApi.Request {
    public class UpdateProductRequest {
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public decimal Price { get; set; }
        public string Sku { get; set; }
        public int Quantity { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsActive { get; set; }
        public List<int> KeepImageIds { get; set; } = [];
        public IFormFileCollection NewImages { get; set; }

        public UpdateProductRequest() {
        }
    }
}
