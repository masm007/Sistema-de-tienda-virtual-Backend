using Application.DTOs.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.OrderDetail {
    public class OrderDetailRequestDto {
        //lo que envia el front
        public int Quantity { get; private set; }
        public string ProductSku { get; set; }

        public OrderDetailRequestDto(string productSku, int quantity) {
            ProductSku = productSku;
            Quantity = quantity;
        }
    }
}
