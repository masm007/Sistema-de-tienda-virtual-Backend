using Application.DTOs.Orders;
using Application.Interfaces.Security;
using Domain.Entity;
using Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Orders {
    public class UpdateOrderUseCase {
        private readonly IOrderRepository<OrderEntity, string> _orderRepository;

        public UpdateOrderUseCase(IOrderRepository<OrderEntity, string> orderRepository) {
            _orderRepository = orderRepository;
        }

        public async Task ExecuteAsync(UpdateOrderDto dto, string orderNumber) {
            var ord = await _orderRepository.GetByOrderNumberForAdminAsync(orderNumber);
            if (ord == null) {
                throw new ArgumentException("No se encontro a una orden con ese número");
            }
            ord.UpdateOrderState(dto.State);
            await _orderRepository.UpdateAsync(ord);
            await _orderRepository.SaveChangesAsync();
        }
    }
}
