using System.ComponentModel.DataAnnotations;

namespace WebApi.Models;

public class User : BaseEntity
{
    [Required, MaxLength(100)]
    public string Email { get; set; }

    [Required, MaxLength(100)]
    public string Password { get; set; }

    public bool IsAdmin { get; set; } = false;
    public DateTime? LastLoginDate { get; set; }
}
