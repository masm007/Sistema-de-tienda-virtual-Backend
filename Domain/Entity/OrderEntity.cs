using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity {
    public class OrderEntity {
        public int Id { get; private set; }
        public string OrderNumber { get; private set; } = string.Empty;
        public DateTime EmisionDate { get; private set; }
        public UserEntity Client { get; private set; }
        public ICollection<OrderDetailsEntity> OrderDetails { get; private set; }
        public decimal Subtotal { get; private set; }
        public decimal Discount { get; private set; }
        public decimal Iva { get; private set; }
        public decimal Total {  get; private set; }
        public OrderStatus State { get; private set; }

        public OrderEntity(string orderNumber, UserEntity client, 
            ICollection<OrderDetailsEntity> orderDetails, int iva = 15) {
            OrderNumber = orderNumber;
            Client = client;
            EmisionDate = DateTime.UtcNow;
            State = OrderStatus.Pending;
            OrderDetails = orderDetails;
            Subtotal = calculateSubtotal(orderDetails);
            Discount = 0;
            Iva = calculateIva(Subtotal, iva);
            Total = calculateTotal(Subtotal, Iva);
        }

        private decimal calculateSubtotal(ICollection<OrderDetailsEntity> orderDetails) {
            decimal subtotal = 0;
            foreach (var item in orderDetails) {
                subtotal += item.Subtotal;
            }
            return subtotal;
        }
        private decimal calculateIva(decimal subtotal, int iva) {
            return (subtotal*iva)/100;
        }
        private decimal calculateTotal(decimal subtotal, decimal iva) {
            return subtotal + iva;
        }

    }
}
