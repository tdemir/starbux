using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Migrations
{
    public partial class m0 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    ParentOrderItemId = table.Column<Guid>(type: "uuid", nullable: true),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    Price = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    ProductType = table.Column<int>(type: "integer", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Price = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    NetPrice = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Price = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    ProductType = table.Column<int>(type: "integer", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserLogins",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Token = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    TokenExpireDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    IsAdmin = table.Column<bool>(type: "boolean", nullable: false),
                    LastLoginDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CreatedDate", "DeletedDate", "Name", "Price", "ProductType" },
                values: new object[,]
                {
                    { new Guid("0e7ca561-a147-460c-98dd-c22556019836"), new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Tea", 3m, 1 },
                    { new Guid("18be78cd-49f7-4a5c-bb7c-b86e9e125d51"), new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Lemon", 2m, 2 },
                    { new Guid("1be28499-751c-4d31-9b90-7307c5448031"), new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Mocha", 6m, 1 },
                    { new Guid("a3af147b-2e43-498a-9f7a-020f98ceb0a4"), new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Hazelnut syrup", 3m, 2 },
                    { new Guid("a44fcd45-1d18-467c-af9a-21de7194d243"), new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Chocolate sauce", 5m, 2 },
                    { new Guid("e49854e8-8b95-47ce-a622-d676e94b3e75"), new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Black Coffee", 4m, 1 },
                    { new Guid("f5ccffc4-1b21-4581-a71d-96bc70791255"), new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Latte", 5m, 1 },
                    { new Guid("ffcdafd8-5906-4414-a364-0d7f1936920b"), new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Milk", 2m, 2 }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedDate", "DeletedDate", "Email", "IsAdmin", "LastLoginDate", "Password" },
                values: new object[,]
                {
                    { new Guid("35062e26-cd41-4123-b824-ea06b0f19596"), new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "admin@test.com", true, null, "202cb962ac59075b964b07152d234b70" },
                    { new Guid("d20733b1-fe94-482c-bd80-d82f7d8d0ef3"), new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "user1@test.com", false, null, "202cb962ac59075b964b07152d234b70" },
                    { new Guid("ed8f558e-1ebb-418c-a1bb-db953ba90c58"), new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "user2@test.com", false, null, "202cb962ac59075b964b07152d234b70" },
                    { new Guid("f2e6040a-df19-4cec-b929-7e2c5c0e53fe"), new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "user3@test.com", false, null, "202cb962ac59075b964b07152d234b70" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email_DeletedDate",
                table: "Users",
                columns: new[] { "Email", "DeletedDate" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "UserLogins");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
