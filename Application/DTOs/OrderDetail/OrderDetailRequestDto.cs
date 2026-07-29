using Application.DTOs.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.OrderDetail {
    public class OrderDetailRequestDto {
        //lo que enviar el front
        public int Quantity { get; private set; }
        public int ProductId { get; set; }

        public OrderDetailRequestDto(int id, int quantity) {
            ProductId = id;
            Quantity = quantity;
        }
    }
}
