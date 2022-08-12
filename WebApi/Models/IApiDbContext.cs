using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace WebApi.Models
{
    public interface IApiDbContext
    {
        DbSet<User> Users { get; set; }
        DbSet<UserLogin> UserLogins { get; set; }
        DbSet<Product> Products { get; set; }
        DbSet<Order> Orders { get; set; }
        DbSet<OrderItem> OrderItems { get; set; }
        //DbSet<OrderItemTopping> OrderItemToppings { get; set; }

        Task<int> SaveChangesAsync();
        int SaveDataChanges();
        IDbContextTransaction BeginTransaction();
    }
}