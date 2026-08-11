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
        public UserEntity User { get; private set; }
        public int UserId { get; private set; }
        public ICollection<OrderDetailEntity> OrderDetails { get; private set; } = new List<OrderDetailEntity>();
        public decimal Subtotal { get; private set; }
        public decimal Discount { get; private set; }
        public decimal Iva { get; private set; }
        public decimal Total {  get; private set; }
        public OrderStatus State { get; private set; }

        //agregar validaciones
        private OrderEntity() { }

        public OrderEntity(int userId, 
            ICollection<OrderDetailEntity> orderDetails, int iva = 15) {
            //OrderNumber = orderNumber;
            UserId = userId;
            State = OrderStatus.Pending;
            OrderDetails = orderDetails;
            Subtotal = CalculateSubtotal(orderDetails);
            Discount = 0;
            Iva = CalculateIva(Subtotal, iva);
            Total = CalculateTotal(Subtotal, Iva);
        }

        private decimal CalculateSubtotal(ICollection<OrderDetailEntity> orderDetails) {
            decimal subtotal = 0;
            foreach (var item in orderDetails) {
                subtotal += item.Subtotal;
            }
            return subtotal;
        }
        private decimal CalculateIva(decimal subtotal, int iva) {
            return (subtotal*iva)/100;
        }
        private decimal CalculateTotal(decimal subtotal, decimal iva) {
            return subtotal + iva;
        }

        public void SetOrderNumber(string orderNumber) {
            OrderNumber = orderNumber;
        }

    }
}
