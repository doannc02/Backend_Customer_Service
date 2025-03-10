using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IChiba.Customer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFieldsIsDefaultAddCustomerAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDefaultAddress",
                table: "CustomerAddresses",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDefaultAddress",
                table: "CustomerAddresses");
        }
    }
}
