using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Microservices.Services.CouponAPI.Migrations
{
    public partial class SeedCouponDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Coupons",
                columns: new[] { "CouponId", "CouponCode", "DiscountAmount" },
                values: new object[] { new Guid("4427e379-4b5c-4d4e-8bab-d3e9b8f6e411"), "10OFF", 10.0 });

            migrationBuilder.InsertData(
                table: "Coupons",
                columns: new[] { "CouponId", "CouponCode", "DiscountAmount" },
                values: new object[] { new Guid("5509ced3-498b-4ae9-8d9d-75fd2334b0f1"), "20OFF", 20.0 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Coupons",
                keyColumn: "CouponId",
                keyValue: new Guid("4427e379-4b5c-4d4e-8bab-d3e9b8f6e411"));

            migrationBuilder.DeleteData(
                table: "Coupons",
                keyColumn: "CouponId",
                keyValue: new Guid("5509ced3-498b-4ae9-8d9d-75fd2334b0f1"));
        }
    }
}
