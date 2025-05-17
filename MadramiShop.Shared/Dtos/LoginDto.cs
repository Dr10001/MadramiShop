using MadramiShop.Shared.Constants;
using System.ComponentModel.DataAnnotations;

namespace MadramiShop.Shared.Dtos;

public class LoginDto
{
    [Required]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }
}
