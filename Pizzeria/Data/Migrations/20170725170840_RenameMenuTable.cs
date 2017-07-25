using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Pizzeria.Data.Migrations
{
    public partial class RenameMenuTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Menu");

            migrationBuilder.CreateTable(
                name: "ProductDb",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Category = table.Column<string>(nullable: false),
                    Components = table.Column<string>(nullable: true),
                    IsInLocal = table.Column<bool>(nullable: false),
                    IsOnline = table.Column<bool>(nullable: false),
                    Price = table.Column<decimal>(nullable: false),
                    ProductName = table.Column<string>(nullable: false),
                    Size = table.Column<double>(nullable: true),
                    SubCategory = table.Column<string>(nullable: true),
                    Weight = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductDb", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductDb");

            migrationBuilder.CreateTable(
                name: "Menu",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Category = table.Column<string>(nullable: false),
                    Components = table.Column<string>(nullable: true),
                    IsInLocal = table.Column<bool>(nullable: false),
                    IsOnline = table.Column<bool>(nullable: false),
                    Price = table.Column<decimal>(nullable: false),
                    ProductName = table.Column<string>(nullable: false),
                    Size = table.Column<double>(nullable: true),
                    SubCategory = table.Column<string>(nullable: true),
                    Weight = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menu", x => x.ID);
                });
        }
    }
}
