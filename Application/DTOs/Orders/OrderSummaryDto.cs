using Application.DTOs.OrderDetail;
using Application.DTOs.Users;
using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Orders {
    public class OrderSummaryDto {
        //se le retorna al usuario para la vista de su orden resumida
        public string OrderNumber { get; private set; } = string.Empty;
        public DateTime EmisionDate { get; private set; }
        public UserDto User { get; private set; }
        public int ProductsQuantity { get; private set; }
        public decimal Total { get; private set; }
        public OrderStatus State { get; private set; }

        public OrderSummaryDto(string orderNumber, DateTime emisionDate, UserDto user,
            int productsQuantity, decimal total, OrderStatus state) {
            OrderNumber = orderNumber;
            EmisionDate = emisionDate;
            User = user;
            ProductsQuantity = productsQuantity;
            Total = total;
            State = state;
        }

        public OrderSummaryDto(string orderNumber, DateTime emisionDate, int productsQuantity,
            decimal total, OrderStatus state) {
            OrderNumber = orderNumber;
            EmisionDate = emisionDate;
            ProductsQuantity = productsQuantity;
            Total = total;
            State = state;
        }
    }
}
