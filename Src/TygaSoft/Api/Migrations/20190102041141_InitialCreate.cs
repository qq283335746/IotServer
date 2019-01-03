using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TygaSoft.Api.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    ApplicationId = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    Id = table.Column<string>(nullable: false),
                    OrderCode = table.Column<string>(nullable: true),
                    ParentOrderCode = table.Column<string>(nullable: true),
                    OrderStatus = table.Column<string>(nullable: true),
                    Latlng = table.Column<string>(nullable: true),
                    LatlngPlace = table.Column<string>(nullable: true),
                    Ip = table.Column<string>(nullable: true),
                    IpPlace = table.Column<string>(nullable: true),
                    Pictures = table.Column<string>(nullable: true),
                    Siblings = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    LastUpdatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PrimaryKey_OrdersId", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    _Id = table.Column<int>(nullable: false),
                    ApplicationId = table.Column<int>(nullable: false),
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    MobilePhone = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    PasswordFormat = table.Column<int>(nullable: false),
                    PasswordSalt = table.Column<string>(nullable: true),
                    Roles = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    LastUpdatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PrimaryKey_UsersId", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
