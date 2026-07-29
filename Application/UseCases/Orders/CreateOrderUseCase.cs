using Application.DTOs.OrderDetail;
using Application.DTOs.Orders;
using Application.DTOs.Products;
using Application.DTOs.Users;
using Application.Interfaces.Storage;
using Domain.Entity;
using Domain.Enum;
using Domain.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Orders {
    public class CreateOrderUseCase {
        private readonly IOrderRepository<OrderEntity, int> _orderRepository;
        private readonly IUserRepository<UserEntity, int> _userRepository;
        private readonly IProductRepository<ProductEntity, int> _productRepository;

        public CreateOrderUseCase(IOrderRepository<OrderEntity, int> orderRepository, 
            IUserRepository<UserEntity, int> userRepository,
            IProductRepository<ProductEntity, int> productRepository) {
            _orderRepository = orderRepository;
            _userRepository = userRepository;
            _productRepository = productRepository;
        }

        public async Task<OrderDto> Execute(CreateOrderDto dto, int id) {
            if (dto == null) {
                throw new ArgumentNullException(nameof(dto));
            }
            if (dto.Details == null || dto.Details.Count == 0) {
                throw new ArgumentException("Debe tener al menos un producto en el carrito");
            }
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) {
                throw new InvalidOperationException("El usuario no existe");
            }
            try {
                // TODO: generar el número de orden
                string orderNumber = "algo";
                List<OrderDetailEntity> orderDetails = new List<OrderDetailEntity>();
                Dictionary<int, ProductEntity> products = new();
                foreach (var item in dto.Details) {
                    var prd = await _productRepository.GetByIdAsync(item.ProductId);
                    if (prd == null) {
                        throw new InvalidOperationException("El producto no existe.");
                    }
                    if (item.Quantity > prd.Quantity) {
                        throw new InvalidOperationException($"No hay suficiente stock de {prd.Name}.");
                    }
                    //Diccionario para ahorrar consultas
                    //products.Add(prd.Id, prd);
                    products[prd.Id] = prd;
                    orderDetails.Add(new OrderDetailEntity(prd.Id, prd.Price, item.Quantity));
                }
                var ord = new OrderEntity(orderNumber, user.Id, orderDetails);
                await _orderRepository.CreateAsync(ord);
                await _orderRepository.SaveChangesAsync();
                //ef core rastrea el id y lo asigna
                var userDto = new UserDto(user.FirstName, user.LastName, user.Email);
                var orderDetailsDto = new List<OrderDetailResponseDto>();
                foreach (var item in ord.OrderDetails) {
                    //var prd = await _productRepository.GetByIdAsync(item.ProductId);
                    // Uso del diccionario
                    ProductEntity prd = products[item.ProductId];
                    var productSummaryDto = new ProductSummaryDto(prd.Id, prd.Name);
                    orderDetailsDto.Add(new OrderDetailResponseDto(productSummaryDto, item.UnitPrice, 
                        item.Quantity, item.Subtotal));
                }
                return new OrderDto(ord.OrderNumber, ord.EmisionDate, userDto, orderDetailsDto,
                    ord.Subtotal, ord.Discount, ord.Iva, ord.Total, ord.State);

            } catch (Exception ex) {
                throw;
            }
        }
    }
}
