using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace WebApi.Models;

public class ApiDbContext : DbContext, IApiDbContext
{
    private readonly IConfiguration _configuration;
    public ApiDbContext(DbContextOptions options, IConfiguration configuration) : base(options)
    {
        _configuration = configuration;
        //this.Database.BeginTransactionAsync();
    }
    public ApiDbContext(DbContextOptions options) : base(options)
    {
        //this.Database.BeginTransactionAsync();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.SetDbContextOptionsBuilder(_configuration);
        }
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.SetDecimalPrecisionAttributeTypes();

        modelBuilder.Entity<User>()
           .HasIndex(p => new { p.Email, p.DeletedDate })
           .IsUnique(true);

        SeedUsers(modelBuilder);
        SeedProducts(modelBuilder);


        base.OnModelCreating(modelBuilder);
    }

    private void SeedUsers(ModelBuilder modelBuilder)
    {
        var encryptedText = "123".MD5Encryption();
        var createdDate = new DateTime(2022, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        modelBuilder.Entity<User>().HasData(new User
        {
            Id = Guid.Parse("35062e26-cd41-4123-b824-ea06b0f19596"),
            Email = "admin@test.com",
            Password = encryptedText,
            IsAdmin = true,
            CreatedDate = createdDate
        }, new User
        {
            Id = Guid.Parse("d20733b1-fe94-482c-bd80-d82f7d8d0ef3"),
            Email = "user1@test.com",
            Password = encryptedText,
            IsAdmin = false,
            CreatedDate = createdDate
        }, new User
        {
            Id = Guid.Parse("ed8f558e-1ebb-418c-a1bb-db953ba90c58"),
            Email = "user2@test.com",
            Password = encryptedText,
            IsAdmin = false,
            CreatedDate = createdDate
        }, new User
        {
            Id = Guid.Parse("f2e6040a-df19-4cec-b929-7e2c5c0e53fe"),
            Email = "user3@test.com",
            Password = encryptedText,
            IsAdmin = false,
            CreatedDate = createdDate
        });
    }

    private void SeedProducts(ModelBuilder modelBuilder)
    {
        var createdDate = new DateTime(2022, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        modelBuilder.Entity<Product>().HasData(new Product
        {
            Id = Guid.Parse("e49854e8-8b95-47ce-a622-d676e94b3e75"),
            Name = "Black Coffee",
            Price = 4m,
            ProductType = Enums.EProductType.Drink,
            CreatedDate = createdDate
        }, new Product
        {
            Id = Guid.Parse("f5ccffc4-1b21-4581-a71d-96bc70791255"),
            Name = "Latte",
            Price = 5m,
            ProductType = Enums.EProductType.Drink,
            CreatedDate = createdDate
        }, new Product
        {
            Id = Guid.Parse("1be28499-751c-4d31-9b90-7307c5448031"),
            Name = "Mocha",
            Price = 6m,
            ProductType = Enums.EProductType.Drink,
            CreatedDate = createdDate
        }, new Product
        {
            Id = Guid.Parse("0e7ca561-a147-460c-98dd-c22556019836"),
            Name = "Tea",
            Price = 3m,
            ProductType = Enums.EProductType.Drink,
            CreatedDate = createdDate
        });

        modelBuilder.Entity<Product>().HasData(new Product
        {
            Id = Guid.Parse("ffcdafd8-5906-4414-a364-0d7f1936920b"),
            Name = "Milk",
            Price = 2m,
            ProductType = Enums.EProductType.Topping,
            CreatedDate = createdDate
        }, new Product
        {
            Id = Guid.Parse("a3af147b-2e43-498a-9f7a-020f98ceb0a4"),
            Name = "Hazelnut syrup",
            Price = 3m,
            ProductType = Enums.EProductType.Topping,
            CreatedDate = createdDate
        }, new Product
        {
            Id = Guid.Parse("a44fcd45-1d18-467c-af9a-21de7194d243"),
            Name = "Chocolate sauce",
            Price = 5m,
            ProductType = Enums.EProductType.Topping,
            CreatedDate = createdDate
        }, new Product
        {
            Id = Guid.Parse("18be78cd-49f7-4a5c-bb7c-b86e9e125d51"),
            Name = "Lemon",
            Price = 2m,
            ProductType = Enums.EProductType.Topping,
            CreatedDate = createdDate
        });
    }

    public Task<int> SaveChangesAsync()
    {
        return base.SaveChangesAsync();
    }

    public int SaveDataChanges()
    {
        return base.SaveChanges();
    }

    public IDbContextTransaction BeginTransaction()
    {
        return this.Database.CurrentTransaction ?? this.Database.BeginTransaction();
    }

    public DbSet<User> Users { get; set; }
    public DbSet<UserLogin> UserLogins { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    //public DbSet<OrderItemTopping> OrderItemToppings { get; set; }
}
