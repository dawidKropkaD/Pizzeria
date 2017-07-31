using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Pizzeria.Data.Migrations
{
    public partial class AddOrderAddress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Order",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FlatNumber",
                table: "Order",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HouseNumber",
                table: "Order",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Street",
                table: "Order",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "FlatNumber",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "HouseNumber",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "Street",
                table: "Order");
        }
    }
}
