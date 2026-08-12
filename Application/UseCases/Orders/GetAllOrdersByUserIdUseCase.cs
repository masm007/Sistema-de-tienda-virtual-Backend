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

        public async Task<IEnumerable<OrderDto>> ExecuteAsync(int userId) {
            var ordersResponse = new List<OrderDto>();
            var orders = await _orderRepository.GetAllByUserIdAsync(userId);
            if (!orders.Any()) {
                return ordersResponse;
            }
            var user = new UserDto(orders.First().User.FirstName, orders.First().User.LastName, 
                orders.First().User.Email);
            foreach (var order in orders) {
                List<OrderDetailResponseDto> orderDetailsResponse = new List<OrderDetailResponseDto>();
                foreach (var item in order.OrderDetails) {
                    var prd = new ProductSummaryDto(item.Product.Id, item.Product.Name);
                    orderDetailsResponse.Add(
                        new OrderDetailResponseDto(prd, item.UnitPrice, item.Quantity, item.Subtotal));
                }
                ordersResponse.Add(new OrderDto(order.OrderNumber, order.EmisionDate, user,
                    orderDetailsResponse, order.Subtotal, order.Discount, order.Iva, 
                    order.Total, order.State));
            }
            return ordersResponse;
        }
    }
}
