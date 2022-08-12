using Microsoft.EntityFrameworkCore;

namespace WebApi.Models
{
    public static class EFCoreExtensionMethods
    {
        public static void SetDbContextOptionsBuilder(this DbContextOptionsBuilder optionsBuilder, IConfiguration configuration)
        {
            var _conStr = configuration.GetConnectionString("DefaultConnection");
            // optionsBuilder.UseSqlServer(_conStr, b => b.MigrationsAssembly("WebApi"));
            optionsBuilder.UseNpgsql(_conStr, b => b.MigrationsAssembly(nameof(WebApi)));
        }
        public static void SetDecimalPrecisionAttributeTypes(this ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property == null || property.PropertyInfo == null)
                    {
                        continue;
                    }
                    var decPrecAttr = property.PropertyInfo.GetCustomAttributes(typeof(Helpers.Attributes.DecimalPrecisionAttribute), false);
                    if (decPrecAttr == null || decPrecAttr.Length < 1)
                    {
                        continue;
                    }
                    var decPrecAttrData = decPrecAttr.First() as Helpers.Attributes.DecimalPrecisionAttribute;
                    if (decPrecAttrData == null)
                    {
                        continue;
                    }
                    //var _columnName = property.GetColumnName();
                    var _columnName = property.PropertyInfo.Name;
                    modelBuilder.Entity(entityType.ClrType)
                                .Property(property.ClrType, _columnName)
                                .HasPrecision(decPrecAttrData.Precision, decPrecAttrData.Scale);

                    // if (Type.GetTypeCode(property.ClrType) == TypeCode.Decimal)
                    // {

                    //     modelBuilder.Entity(entityType.ClrType).Property(property.ClrType, property.GetColumnName()).HasPrecision(10, 3);
                    // }
                }
            }
        }
    }
}