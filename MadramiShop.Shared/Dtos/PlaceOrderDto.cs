using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MadramiShop.Shared.Dtos;

public record PlaceOrderDto(int UserAddressId,string Address, string AddressName, OrderItemSaveDto[] Items);

public class ChangePasswordDto
{
    [Required]
    public string CurrentPassword { get; set; }
    [Required]
    public string NewPassword { get; set; }
    [Required, Compare(nameof(NewPassword))]
    [JsonIgnore]
    public string ConfirmNewPassword { get; set; }

}
