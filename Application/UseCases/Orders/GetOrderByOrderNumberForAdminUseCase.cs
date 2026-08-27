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
    public class GetOrderByOrderNumberForAdminUseCase {
        private readonly IOrderRepository<OrderEntity, string> _orderRepository;

        public GetOrderByOrderNumberForAdminUseCase(IOrderRepository<OrderEntity, string> orderRepository) {
            _orderRepository = orderRepository;
        }

        public async Task<OrderDto> ExecuteAsync(string orderNumber) {
            var ord = await _orderRepository.GetByOrderNumberForAdminAsync(orderNumber);
            if (ord == null) {
                throw new InvalidOperationException("Orden no encontrada");
            }
            var userResponse = new UserDto(ord.User.FirstName, ord.User.LastName, ord.User.Email);
            var orderDetailsResponse = new List<OrderDetailResponseDto>();
            foreach (var item in ord.OrderDetails) {
                var prd = new ProductSummaryDto(item.Product.Id, item.Product.Name);
                orderDetailsResponse.Add(
                    new OrderDetailResponseDto(prd, item.UnitPrice, item.Quantity, item.Subtotal));
            }
            var response = new OrderDto(ord.OrderNumber, ord.EmisionDate, userResponse,
                orderDetailsResponse, ord.Subtotal, ord.Discount, ord.Iva, ord.Total, ord.State);
            return response;
        }
    }
}
