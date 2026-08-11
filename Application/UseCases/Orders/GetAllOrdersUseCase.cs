using Domain.Entity;
using Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Orders {
    public class GetAllOrdersUseCase {
        private readonly IOrderRepository<OrderEntity, string> _orderRepository;

        public GetAllOrdersUseCase(IOrderRepository<OrderEntity, string> orderRepository) {
            _orderRepository = orderRepository;
        }
    }
}
