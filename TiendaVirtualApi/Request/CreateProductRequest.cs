using Application.DTOs.Images;

namespace TiendaVirtualApi.Request {
    public class CreateProductRequest {
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public decimal Price { get; set; }
        public string Sku { get; set; }
        public int Quantity { get; set; }
        public IFormFileCollection Images { get; set; }

        public CreateProductRequest() {
        }
    }
}
