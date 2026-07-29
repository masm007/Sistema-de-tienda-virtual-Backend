using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity {
    public class OrderDetailEntity {
        public int Id { get; private set; }
        public int OrderId { get; private set; }
        public OrderEntity Order { get; private set; }
        public int Quantity { get; private set; }
        public int ProductId {  get; private set; }
        public ProductEntity Product { get; private set; }
        public decimal UnitPrice { get; private set; }
        public decimal Subtotal { get; private set; }

        private OrderDetailEntity() {
        }

        public OrderDetailEntity(int productId, decimal unitPrice, int quantity) {
            //OrderId = orderId;
            ProductId = productId;
            Quantity = quantity;
            UnitPrice = unitPrice;
            Subtotal = CalculateSubtotal(unitPrice, quantity);
        }

        private decimal CalculateSubtotal(decimal unitPrice, int quantity) {
            return unitPrice * quantity;
        }

    }
}
