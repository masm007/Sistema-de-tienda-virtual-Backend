using Application.DTOs.OrderDetail;

namespace TiendaVirtualApi.Request {
    public class ValidateCouponRequest {
        public string Code { get; set; }
        public List<OrderDetailRequestDto> Details { get; set; } = [];

        public ValidateCouponRequest() { }
    }
}
