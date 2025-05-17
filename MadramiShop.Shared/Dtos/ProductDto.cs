using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MadramiShop.Shared.Dtos;

public class ProductDto
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string ImageUrl { get; set; }

    public decimal Price { get; set; }

    public string Unit { get; set; }

    [JsonIgnore]//just for UI
    public int Quantity { get; set; }

}

public class AddressDto 
{
    public int Id { get; set; }
    [Required]
    public string Address { get; set; }
    [Required]
    public string Name { get; set; }
    public bool IsDefault { get; set; }

}