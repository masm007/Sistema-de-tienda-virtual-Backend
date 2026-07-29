using Application.DTOs.OrderDetail;
using Application.DTOs.Users;
using Domain.Entity;
using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Orders {
    public class OrderDto {
        //se le retorna al usuario para la vista de su orden
        public string OrderNumber { get; private set; } = string.Empty;
        public DateTime EmisionDate { get; private set; }
        public UserDto User { get; private set; }
        public List<OrderDetailResponseDto> OrderDetails { get; private set; } 
            = new List<OrderDetailResponseDto>();
        public decimal Subtotal { get; private set; }
        public decimal Discount { get; private set; }
        public decimal Iva { get; private set; }
        public decimal Total { get; private set; }
        public OrderStatus State { get; private set; }

        public OrderDto(string orderNumber, DateTime emisionDate, UserDto user,
            List<OrderDetailResponseDto> orderDetails, decimal subtotal, decimal discount,
            decimal iva, decimal total, OrderStatus state) {
            OrderNumber = orderNumber;
            EmisionDate = emisionDate;
            User = user;
            OrderDetails = orderDetails;
            Subtotal = subtotal;
            Discount = discount;
            Iva = iva;
            Total = total;
            State = state;
        }
    }
}
