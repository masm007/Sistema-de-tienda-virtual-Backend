using Application.DTOs.Products;
using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.OrderDetail {
    public class OrderDetailResponseDto {
        //se enviara a la orden en el front
        public int Quantity { get; private set; }
        public ProductSummaryDto Product { get; private set; }
        public decimal UnitPrice { get; private set; }
        public decimal Subtotal { get; private set; }

        public OrderDetailResponseDto(ProductSummaryDto prd, decimal unitPrice, 
            int quantity, decimal subtotal) {
            //OrderId = orderId;
            Product = prd;
            Quantity = quantity;
            UnitPrice = unitPrice;
            Subtotal = subtotal;
        }
    }
}
