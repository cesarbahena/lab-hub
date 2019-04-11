using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuimiosHub.Migrations
{
    public partial class AddPerformanceIndexes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_shift_handovers_handover_date",
                table: "shift_handovers",
                column: "handover_date");

            migrationBuilder.CreateIndex(
                name: "IX_samples_cliente_grd",
                table: "samples",
                column: "cliente_grd");

            migrationBuilder.CreateIndex(
                name: "IX_samples_fecha_recep",
                table: "samples",
                column: "fecha_recep");

            migrationBuilder.CreateIndex(
                name: "IX_inventory_items_category",
                table: "inventory_items",
                column: "category");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_shift_handovers_handover_date",
                table: "shift_handovers");

            migrationBuilder.DropIndex(
                name: "IX_samples_cliente_grd",
                table: "samples");

            migrationBuilder.DropIndex(
                name: "IX_samples_fecha_recep",
                table: "samples");

            migrationBuilder.DropIndex(
                name: "IX_inventory_items_category",
                table: "inventory_items");
        }
    }
}
