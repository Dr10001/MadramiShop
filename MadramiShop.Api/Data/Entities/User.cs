using System.ComponentModel.DataAnnotations;

namespace MadramiShop.Api.Data.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(20)]
        public string Name { get; set; }

        [Required, MaxLength(150)]
        public string Email { get; set; }

        [MaxLength(20)]
        public string? Mobile { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        public ICollection<UserAddress> UserAddresses { get; set; }
    }

}
