using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuimiOSHub.Migrations
{
    /// <inheritdoc />
    public partial class AddValidationCancellationUnidentifiedConsumption : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "cancellation_consumption",
                table: "consumption_records",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "unidentified_consumption",
                table: "consumption_records",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "validation_consumption",
                table: "consumption_records",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "cancellation_consumption",
                table: "consumption_records");

            migrationBuilder.DropColumn(
                name: "unidentified_consumption",
                table: "consumption_records");

            migrationBuilder.DropColumn(
                name: "validation_consumption",
                table: "consumption_records");
        }
    }
}
