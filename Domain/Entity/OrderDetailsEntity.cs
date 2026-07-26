using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity {
    public class OrderDetailsEntity {
        public int Quantity { get; set; }
        public ProductEntity Product { get; private set; }
        public decimal UnitPrice { get; private set; }
        public decimal Subtotal => UnitPrice * Quantity;

        public OrderDetailsEntity(ProductEntity product, int quantity) {
            Product = product;
            Quantity = quantity;
            UnitPrice = product.Price;
        }

    }
}
