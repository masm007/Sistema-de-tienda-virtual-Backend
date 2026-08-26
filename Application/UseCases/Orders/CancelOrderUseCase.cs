using Application.Interfaces.Storage;
using Domain.Entity;
using Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Orders {
    public class CancelOrderUseCase {
        private readonly IOrderRepository<OrderEntity, string> _orderRepository;

        public CancelOrderUseCase(IOrderRepository<OrderEntity, string> orderRepository) {
            _orderRepository = orderRepository;
        }

        public async Task ExecuteAsync(string orderNumber) {
            var ord = await _orderRepository.GetByOrderNumberForAdminAsync(orderNumber);
            if (ord == null) {
                throw new InvalidOperationException("Orden no encontrado");
            }
            await _orderRepository.CancelOrderAsync(ord);
        }
    }
}
