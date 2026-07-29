using Application.DTOs.Images;
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
    public class CreateOrderDto {
        //se envia al back el usuario y los detalles de la orden
        public List<OrderDetailRequestDto> Details { get; private set; } = [];

        public CreateOrderDto(List<OrderDetailRequestDto> details) {
            Details = details;
        }
    }
}
