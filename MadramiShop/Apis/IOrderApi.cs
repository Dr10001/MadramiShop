using MadramiShop.Shared.Dtos;
using Refit;

namespace MadramiShop.Apis;

[Headers("Authorization: Bearer ")]
public interface IOrderApi
{
    [Post("/api/orders/place-order")]
    Task<ApiResult> PlaceOrderAsync(PlaceOrderDto dto);

    [Get("/api/orders/users/{userId}")]
    Task<OrderDto[]> GetUserOrderAsync(int userId, int startIndex, int pageSize);

    [Get("/api/orders/users/{userId}/orders/{orderId}/items")]
    Task<ApiResult<OrderItemDto[]>> GetUserOrderItemsAsync(int orderId, int userId);
}
