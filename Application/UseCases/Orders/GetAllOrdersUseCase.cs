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
    public class GetAllOrdersUseCase {
        private readonly IOrderRepository<OrderEntity, string> _orderRepository;

        public GetAllOrdersUseCase(IOrderRepository<OrderEntity, string> orderRepository) {
            _orderRepository = orderRepository;
        }

        public async Task<IEnumerable<OrderDto>> ExecuteAsync() {
            var ordersResponse = new List<OrderDto>();
            var orders = await _orderRepository.GetAllAsync();
            if (!orders.Any()) {
                return ordersResponse;
            }
            foreach (var order in orders) {
                var user = new UserDto(order.User.FirstName, order.User.LastName, order.User.Email);
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
