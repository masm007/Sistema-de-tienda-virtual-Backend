using Application.DTOs.Orders;
using Application.DTOs.Users;
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

        public async Task<IEnumerable<OrderSummaryDto>> ExecuteAsync() {
            var ordersResponse = new List<OrderSummaryDto>();
            var orders = await _orderRepository.GetAllAsync();
            if (!orders.Any()) {
                return ordersResponse;
            }
            foreach (var order in orders) {
                var user = new UserDto(order.User.FirstName, order.User.LastName, order.User.Email);
                int productsQuantity = order.OrderDetails.Sum(detail => detail.Quantity);
                ordersResponse.Add(new OrderSummaryDto(order.OrderNumber, order.EmisionDate, user,
                    productsQuantity, order.Total, order.State));
            }
            return ordersResponse;
        }
    }
}
