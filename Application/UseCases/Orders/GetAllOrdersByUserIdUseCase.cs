using Domain.Entity;
using Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Orders {
    public class GetAllOrdersByUserIdUseCase {
        private readonly IOrderRepository<OrderEntity, int> _orderRepository;

        public GetAllOrdersByUserIdUseCase(IOrderRepository<OrderEntity, int> orderRepository) {
            _orderRepository = orderRepository;
        }
    }
}
