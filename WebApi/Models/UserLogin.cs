using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class UserLogin : BaseEntity
    {
        public Guid UserId { get; set; }

        [MaxLength(1000)]
        public string Token { get; set; }

        public DateTime TokenExpireDate { get; set; }

    }
}