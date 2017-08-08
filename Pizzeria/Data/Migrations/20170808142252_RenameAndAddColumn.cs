using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Pizzeria.Data.Migrations
{
    public partial class RenameAndAddColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Value",
                table: "OrderedProduct",
                newName: "FinalValue");

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "OrderedProduct",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "OrderedProduct");

            migrationBuilder.RenameColumn(
                name: "FinalValue",
                table: "OrderedProduct",
                newName: "Value");
        }
    }
}
