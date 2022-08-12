using System.ComponentModel.DataAnnotations;

namespace WebApi.Models;

public abstract class BaseEntity
{
    [Key]
    public Guid Id { get; set; }

    public BaseEntity()
    {
        Id = Guid.NewGuid();
        CreatedDate = DateTime.UtcNow;
    }


    public DateTime CreatedDate { get; set; }
    public DateTime? DeletedDate { get; set; }
}
