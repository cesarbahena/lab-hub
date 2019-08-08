using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace QuimiOSHub.Migrations
{
    /// <inheritdoc />
    public partial class AddRealInventorySystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "consumption_record_id",
                table: "inventory_movements",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "reagent_id",
                table: "inventory_items",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "reagents",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    calibration_consumption = table.Column<int>(type: "integer", nullable: false),
                    category = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    unit = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reagents", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "consumption_records",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    reagent_id = table.Column<int>(type: "integer", nullable: false),
                    consumption_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    research_consumption = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    repeat_consumption = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    qc_consumption = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    manual_consumption = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    calibration_consumption = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    total_consumption = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_consumption_records", x => x.id);
                    table.ForeignKey(
                        name: "FK_consumption_records_reagents_reagent_id",
                        column: x => x.reagent_id,
                        principalTable: "reagents",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_inventory_movements_consumption_record_id",
                table: "inventory_movements",
                column: "consumption_record_id");

            migrationBuilder.CreateIndex(
                name: "IX_inventory_items_reagent_id",
                table: "inventory_items",
                column: "reagent_id");

            migrationBuilder.CreateIndex(
                name: "IX_consumption_records_consumption_date",
                table: "consumption_records",
                column: "consumption_date");

            migrationBuilder.CreateIndex(
                name: "IX_consumption_records_reagent_id",
                table: "consumption_records",
                column: "reagent_id");

            migrationBuilder.CreateIndex(
                name: "IX_reagents_code",
                table: "reagents",
                column: "code",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_inventory_items_reagents_reagent_id",
                table: "inventory_items",
                column: "reagent_id",
                principalTable: "reagents",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_inventory_movements_consumption_records_consumption_record_~",
                table: "inventory_movements",
                column: "consumption_record_id",
                principalTable: "consumption_records",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_inventory_items_reagents_reagent_id",
                table: "inventory_items");

            migrationBuilder.DropForeignKey(
                name: "FK_inventory_movements_consumption_records_consumption_record_~",
                table: "inventory_movements");

            migrationBuilder.DropTable(
                name: "consumption_records");

            migrationBuilder.DropTable(
                name: "reagents");

            migrationBuilder.DropIndex(
                name: "IX_inventory_movements_consumption_record_id",
                table: "inventory_movements");

            migrationBuilder.DropIndex(
                name: "IX_inventory_items_reagent_id",
                table: "inventory_items");

            migrationBuilder.DropColumn(
                name: "consumption_record_id",
                table: "inventory_movements");

            migrationBuilder.DropColumn(
                name: "reagent_id",
                table: "inventory_items");
        }
    }
}
