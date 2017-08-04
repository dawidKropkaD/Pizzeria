using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Pizzeria.Data.Migrations
{
    public partial class ChangeOrderedProductTableStructure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "OrderedProduct");

            migrationBuilder.AddColumn<string>(
                name: "Components",
                table: "OrderedProduct",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "OrderedProduct",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Size",
                table: "OrderedProduct",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Weight",
                table: "OrderedProduct",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Components",
                table: "OrderedProduct");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "OrderedProduct");

            migrationBuilder.DropColumn(
                name: "Size",
                table: "OrderedProduct");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "OrderedProduct");

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "OrderedProduct",
                nullable: false,
                defaultValue: 0);
        }
    }
}
