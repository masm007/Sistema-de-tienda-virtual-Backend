using Application.DTOs.OrderDetail;
using Application.DTOs.Users;
using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Orders {
    public class UpdateOrderDto {
        //envia el admin para editar un estado de orden
        public OrderStatus State { get; private set; }

        public UpdateOrderDto(OrderStatus state) {
            State = state;
        }
    }
}
