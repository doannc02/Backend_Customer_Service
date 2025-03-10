using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IChiba.Customer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFieldsCustomerAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "CustomerAddresses",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateAt",
                table: "CustomerAddresses",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "CreateBy",
                table: "CustomerAddresses",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeleteAt",
                table: "CustomerAddresses",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeleteBy",
                table: "CustomerAddresses",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "CustomerAddresses",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "CustomerAddresses",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateAt",
                table: "CustomerAddresses",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UpdateBy",
                table: "CustomerAddresses",
                type: "uuid",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Country",
                table: "CustomerAddresses");

            migrationBuilder.DropColumn(
                name: "CreateAt",
                table: "CustomerAddresses");

            migrationBuilder.DropColumn(
                name: "CreateBy",
                table: "CustomerAddresses");

            migrationBuilder.DropColumn(
                name: "DeleteAt",
                table: "CustomerAddresses");

            migrationBuilder.DropColumn(
                name: "DeleteBy",
                table: "CustomerAddresses");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "CustomerAddresses");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "CustomerAddresses");

            migrationBuilder.DropColumn(
                name: "UpdateAt",
                table: "CustomerAddresses");

            migrationBuilder.DropColumn(
                name: "UpdateBy",
                table: "CustomerAddresses");
        }
    }
}
