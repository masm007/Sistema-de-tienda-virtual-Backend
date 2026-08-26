using Application.DTOs.OrderDetail;
using Application.DTOs.Orders;
using Application.DTOs.Products;
using Application.DTOs.Users;
using Domain.Entity;
using Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Orders {
    public class GetAllOrdersByUserIdUseCase {
        private readonly IOrderRepository<OrderEntity, string> _orderRepository;

        public GetAllOrdersByUserIdUseCase(IOrderRepository<OrderEntity, string> orderRepository) {
            _orderRepository = orderRepository;
        }

        public async Task<IEnumerable<OrderSummaryDto>> ExecuteAsync(int userId) {
            var ordersResponse = new List<OrderSummaryDto>();
            var orders = await _orderRepository.GetAllByUserIdAsync(userId);
            if (!orders.Any()) {
                return ordersResponse;
            }
            foreach (var order in orders) {
                int productsQuantity = order.OrderDetails.Sum(detail => detail.Quantity);
                ordersResponse.Add(new OrderSummaryDto(order.OrderNumber, order.EmisionDate,
                    productsQuantity, order.Total, order.State));
            }
            return ordersResponse;
        }
    }
}
